using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Areas.Principal.Models.Charts;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class InstituicaoController : ListagemCursosController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoCurso _servicoCurso;
        private ServicoInstituicao _servicoInstituicao;
        readonly IHostingEnvironment env;

        public InstituicaoController(
            UserManager<UsuarioADE> userManager,
            IHostingEnvironment _env,
            SignInManager<UsuarioADE> signInManager,
            ApplicationDbContext _context
        ): base(userManager, signInManager, _context, _env)
        {
            context = _context;
            unitOfWork = new UnitOfWork(context);
            env = _env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool Partial = false, int? pageNumber = 1)
        {
            try
            {
                if (Partial)
                    return PartialView("_Index", await ParseInstituicaoView(pageNumber));

                return View(await ParseInstituicaoView(pageNumber));
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Error", "HttpStatusCodeHandler", "401");
            }
        }
        [HttpGet]
        public async Task<IActionResult> VisualizacaoInstituicao(int idInstituicao)
        {
            try
            {
                return View(await ParseVisualizacaoInstituicaoView(idInstituicao));
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Error", "HttpStatusCodeHandler", "401");
            }
        }

        [HttpGet]
        public async Task<IActionResult> TrocarInstituicao(int idInstituicao)
        {
            try
            {
                _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
                Instituicao instituicao = await _servicoInstituicao.BuscarPorId(idInstituicao);
                UsuarioADE usuario = await ObterUsuarioLogado();
                usuario.Email = User.Identity.Name;
                usuario.Id = usuario.Id;
                usuario.UserName = usuario.UserName;
                usuario.IdInstituicao = instituicao.Identificador;
                await AtualizarUsuario(usuario);
                ViewBag.Retorno = $"Instituicao alterada para {instituicao.Nome}";
                return View("VisualizacaoInstituicao", await ParseVisualizacaoInstituicaoView(idInstituicao));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Ocorreu um erro alterar o a Instituição do usuário");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.AlteracaoInstituicaoUsuario);
                return RedirectToAction("Index", "Account");
            }
        }

        private async Task<InformacaoInstituicao> ParseVisualizacaoInstituicaoView(int idInstituicao)
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            Instituicao instituicao;
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            
            if (usuario.IdInstituicao > 0 && idInstituicao == 0)
                instituicao = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
            else
                instituicao = await _servicoInstituicao.BuscarPorId(idInstituicao);
            instituicao.Cursos = await _servicoCurso.Filtrar(x => x.IdInstituicao == instituicao.Identificador);
            InformacaoInstituicao InfoInstituicao = await ObterInformacaoInstituicao(instituicao, usuario, unitOfWork);
            return InfoInstituicao; 
        }

        private async Task<InstituicaoViewModel> ParseInstituicaoView(int? pageNumber)
        {
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            InstituicaoViewModel model = new InstituicaoViewModel()
            {
                Usuario = usuario,
                PaginaInstituicoes = await ObterPaginaInstituicao(pageNumber, usuario, unitOfWork),
                Instituicoes = await _servicoInstituicao.ListarAsync()
            };

            if (!usuario.PossuiInstituicao())
                model.PrimeiraInstituicao = true;
            else 
                usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
                
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> ObterResultadoPesquisaInstituicao(string Nome)
        {
            try
            {
                _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
                List<Instituicao> instituicoes = await _servicoInstituicao.Filtrar(x => x.Nome == Nome);
                return PartialView("_SelecaoInstituicao_InstituicaoOption", instituicoes);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Ocorreu um erro ao buscar instituições compatíveis com a pesquisa");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                List<Instituicao> lista = new List<Instituicao>()
                {
                    new Instituicao(){Nome = "Não encontrado",Identificador =1}
                };
                return PartialView("_SelecaoInstituicao_InstituicaoOption", lista);
            }
        }

        [HttpGet]
        public async Task<PaginatedList<InformacaoInstituicao>> ObterPaginaInstituicao(int? PageNumber, UsuarioADE usuario, UnitOfWork unitOfWork = null)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);

            List<Instituicao> instituicoes = await _servicoInstituicao.ListarAsync();
            List<InformacaoInstituicao> infoInstituicoes = await ObterInformacaoInstituicao(instituicoes, usuario);
            return PaginatedList<InformacaoInstituicao>.Create(infoInstituicoes.AsQueryable(), PageNumber ?? 1, 9);
        }

        private async Task<InformacaoInstituicao> ObterInformacaoInstituicao(Instituicao instituicao, UsuarioADE usuario, UnitOfWork unitOf)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);

            InformacaoInstituicao infoInstituicao = new InformacaoInstituicao()
            {
                QuantidadeCursosInstituicao = _servicoCurso.Filtrar(x => x.IdInstituicao == instituicao.Identificador).Result.Count,
                QuantidadeAlunosInstituicao = await CountUsuarioByInstituicao(instituicao.Identificador),
                Instituicao = instituicao,
                InstituicaoDoUsuario = usuario.IdInstituicao == instituicao.Identificador ? true : false
            };
            return infoInstituicao;
        }

        private async Task<List<InformacaoInstituicao>> ObterInformacaoInstituicao(List<Instituicao> instituicoes, UsuarioADE usuario)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            List<InformacaoInstituicao> infoInstituicoes = new List<InformacaoInstituicao>();
            foreach (Instituicao instituicao in instituicoes)
            {
                int QuantidadeCursos = _servicoCurso.Filtrar(x => x.IdInstituicao == instituicao.Identificador).Result.Count;
                int QuantidadeAlunos = await CountUsuarioByInstituicao(instituicao.Identificador);
                bool InstituicaoDoUsuario = usuario.IdInstituicao == instituicao.Identificador ? true : false;
                InformacaoInstituicao infoInstituicao = new InformacaoInstituicao(instituicao, QuantidadeAlunos, QuantidadeCursos, InstituicaoDoUsuario);
                infoInstituicoes.Add(infoInstituicao);
            }
            return infoInstituicoes;
        }

    }
}