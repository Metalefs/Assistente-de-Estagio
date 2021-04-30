using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Utilidades.Extensions;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Assistente_de_Estagio.Areas.Ajuda.Models;
using ADE.Dominio.Models.Enums;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Ajuda")]
    public class FAQController : BaseController
    {
        readonly ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoFAQ _servicoFAQ;

        public FAQController(
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
            ) : base(new UnitOfWork(_context), userManager, signInManager)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
            _servicoFAQ = new ServicoFAQ(ref unitOfWork, userManager);
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int IdInstituicao = 0, string searchString = null)
        {
            try
            {
                if (UsuarioValido())
                {
                    FAQViewModel model = await ParseFAQViewModel(pageNumber, IdInstituicao, searchString);
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Falha", "Usuário não está autenticado");
                    return RedirectToAction("Logout", "Account");
                }
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        private async Task<FAQViewModel> ParseFAQViewModel(int pageNumber, int IdInstituicao, string searchString = null)
        {
            if(null != searchString)
            {
                return await PerguntasFiltradas(pageNumber,searchString);
            }
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            UsuarioADE usuario = await ObterUsuarioLogado();
            IdInstituicao = IdInstituicao == 0 ? usuario.IdInstituicao : IdInstituicao;
            List<FAQ> FAQs = new List<FAQ>();
            List<InformacaoFAQ> InfoFAQs = new List<InformacaoFAQ>();
            if (User.IsAdminOrCriadorConteudo())
            {
                FAQs = await _servicoFAQ.BuscarPorIdInstituicaoAdmin(IdInstituicao);
                InfoFAQs = await ParseInformacaoFAQ(FAQs);
            }
            else
            {
                FAQs = await _servicoFAQ.BuscarPorIdInstituicao(IdInstituicao);
                InfoFAQs = await ParseInformacaoFAQ(FAQs);
            }
            PaginatedList<InformacaoFAQ> PFAQs = PaginatedList<InformacaoFAQ>.Create(InfoFAQs.AsQueryable(), pageNumber, 10);
            FAQViewModel model = new FAQViewModel()
            {
                FAQS = PFAQs,
                QuantidadePerguntas = FAQs.Count,
                QuantidadePerguntasNaoRespondidas = await _servicoFAQ.CountByFilter(x=> x.Status != EnumStatusFAQ.Respondido),
                QuantidadePerguntasRespondidas = await _servicoFAQ.CountByFilter(x=> x.Status == EnumStatusFAQ.Respondido),
                Instituicao = await new ServicoInstituicao(ref unitOfWork).BuscarPorId(IdInstituicao)
            };
            return model;
        }

        private async Task<FAQViewModel> PerguntasFiltradas(int pageNumber, string searchString)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            UsuarioADE usuario = await ObterUsuarioLogado();
            List<FAQ> FAQs = await _servicoFAQ.Filtrar(x => x.Pergunta.Contains(searchString));
            List<InformacaoFAQ> InfoFAQs = await ParseInformacaoFAQ(FAQs);
            PaginatedList<InformacaoFAQ> PFAQs = PaginatedList<InformacaoFAQ>.Create(InfoFAQs.AsQueryable(), pageNumber, 10);
            FAQViewModel model = new FAQViewModel()
            {
                FAQS = PFAQs,
                Instituicao = await new ServicoInstituicao(ref unitOfWork).BuscarPorId(usuario.IdInstituicao)
            };
            return model;
        }

        private async Task<List<InformacaoFAQ>> ParseInformacaoFAQ(List<FAQ> FAQs)
        {
            List<InformacaoFAQ> informacaoFAQs = new List<InformacaoFAQ>(); 
            foreach(FAQ FAQ in FAQs)
            {
                UsuarioADE UsuarioPergunta = await ObterUsuarioPorEmailOuId(FAQ.IdUsuarioPergunta);
                UsuarioADE UsuarioResposta = await ObterUsuarioPorEmailOuId(FAQ.IdUsuarioResposta);
                InformacaoFAQ InformacaoFAQ = new InformacaoFAQ(FAQ, UsuarioPergunta, UsuarioResposta);
                informacaoFAQs.Add(InformacaoFAQ);
            }
            return informacaoFAQs;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarPergunta(string Pergunta, int IdInstituicao)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                UsuarioADE usuario = await ObterUsuarioLogado();
                FAQ FAQ = new FAQ(IdInstituicao,Pergunta,"", usuario.Id, usuario.Id);
                await _servicoFAQ.CadastrarAsync(usuario, FAQ);
                Instituicao instituicao = await new ServicoInstituicao(ref unitOfWork).BuscarPorId(IdInstituicao);
                await NotificarAdminsAdicaoPergunta(Pergunta, usuario.Id, instituicao);
                return Json("{\"Sucesso\": \"Sua pergunta foi colocada em revisão.\"}");
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoFAQ);
                return Json("{\"Erro\": \"Erro ao adicionar pergunta\"}");
            }
        }

        private async Task NotificarAdminsAdicaoPergunta(string Pergunta, string IDUsuarioPergunta, Instituicao instituicao)
        {
            ServicoInformacaoDocumento servico = new ServicoInformacaoDocumento(ref unitOfWork);
            IList<UsuarioADE> Admins = await ObterUsuariosPorFuncao(EnumTipoUsuario.CriadorConteudo.GetDescription());
            List<NotificacaoIndividual> ListaNotificoes = new List<NotificacaoIndividual>();
            foreach (UsuarioADE Admin in Admins)
            {
                NotificacaoIndividual Notificacao = new NotificacaoIndividual(IDUsuarioPergunta, Admin.Id, $"Nova Pergunta ({Pergunta}) adicionada a instituição {instituicao.ToString()}", $"Nova Pergunta ({Pergunta}) adicionada a instituição {instituicao.ToString()}", EnumStatusNotificacaoIndividual.Enviado);
                await servico.CriarNotificacaoIndividual(Admin, Notificacao);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarResposta(int IdFAQ, string Resposta, int IdInstituicao)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                if (User.IsAdminOrCriadorConteudo())
                {
                    UsuarioADE usuario = await ObterUsuarioLogado();
                    FAQ faq = await _servicoFAQ.BuscarPorId(IdFAQ);
                    faq.Resposta = Resposta;
                    faq.IdUsuarioResposta = usuario.Id;
                    faq.Status = EnumStatusFAQ.Respondido;
                    await _servicoFAQ.AtualizarAsync(usuario, faq);
                    return Json("{\"Sucesso\": \"Sua resposta foi adicionada.\"}");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoFAQ);
                return Json("{\"Erro\": \"Erro ao adicionar resposta\"}");
            }
        }

        [HttpPost]
        public async Task<int> Like(int IdFAQ)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                UsuarioADE usuario = await ObterUsuarioLogado();
                return await _servicoFAQ.Like(usuario, IdFAQ);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoFAQ);
                return 0;
            }
        }
        [HttpPost]
        public async Task<int> Dislike(int IdFAQ)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                UsuarioADE usuario = await ObterUsuarioLogado();
                return await _servicoFAQ.Dislike(usuario, IdFAQ);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoFAQ);
                return 0;
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,CriadorConteudo")]
        public async Task Deletar(int IdFAQ)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                UsuarioADE usuario = await ObterUsuarioLogado();
                FAQ FAQ = await _servicoFAQ.BuscarPorId(IdFAQ);
                FAQ.Status = EnumStatusFAQ.Rejeitado;
                await _servicoFAQ.RemoverAsync(usuario, FAQ);
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.CriacaoFAQ);
            }
        }

    }
}