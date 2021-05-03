using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
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
using Newtonsoft.Json;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class MeusDadosController : BaseController
    {
        private ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoRequisitoUsuario _servicoRequisitoUsuario;
        private ServicoRequisito _servicoRequisito;
        private ServicoInstituicao _servicoInstituicao;
        readonly IHostingEnvironment env;

        public MeusDadosController(
            UserManager<UsuarioADE> userManager,
            IHostingEnvironment _env,
            ApplicationDbContext _context
        ): base(new UnitOfWork(_context), userManager)
        {
            context = _context;
            env = _env;
            unitOfWork = new UnitOfWork(context);
        }

        public async Task<IActionResult> Index(bool Partial = false)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            UsuarioADE usuario = await ObterUsuarioComDadosPessoais();
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);

            usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
            MeusDadosViewModel model = new MeusDadosViewModel()
            {
                Usuario = usuario,
                RequisitoDoUsuario = await ObterPaginaRequisitoUsuario(1, usuario.Id, unitOfWork)
            };
            if (Partial)
                return PartialView("_Index", model);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AtualizarDados(string DadosAluno, bool Estagiando, int IdArea)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                ServicoRequisito _servicoRequisito = new ServicoRequisito(ref unitOfWork);
                _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
                List<DadosAlunoKV> dadosAluno = JsonConvert.DeserializeObject<List<DadosAlunoKV>>(DadosAluno);
                foreach (DadosAlunoKV dado in dadosAluno)
                {
                    string id = dado.Name.Split(':')[1];
                    dado.Requisito = await _servicoRequisito.BuscarUm(x => x.Bookmark == id);
                }
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (usuario.Id != "N/A")
                {
                    await _servicoRequisitoUsuario.AdicionarRequisitosDeUsuarioAsync(dadosAluno, usuario);
                    if(IdArea != 0)
                    {
                        await CadastrarAreaEstagioAluno(usuario, IdArea);
                    }
                    usuario.Estagiando = Estagiando;
                    await AtualizarUsuario(usuario);
                }
                return PartialView("_FormularioRequisitosBasicos");
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoRequisitoDeUsuario);
                return Json("{\"Erro\": \"Erro ao atualizar informações\"}");
            }
        }

        private async Task CadastrarAreaEstagioAluno(UsuarioADE usuario, int idArea)
        {
            ServicoAreaEstagioCurso _servicoAreaEstagio = new ServicoAreaEstagioCurso(ref unitOfWork);
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            Requisito req = await _servicoRequisito.BuscarUm(x => x.Bookmark == "_AreasEstagio_");
            AreaEstagioCurso area = await _servicoAreaEstagio.BuscarUm(x => x.Identificador == idArea);
            RequisitoDeUsuario requisitoDeUsuario = new RequisitoDeUsuario();
            requisitoDeUsuario.IdRequisito = req.Identificador;
            requisitoDeUsuario.UserId = usuario.Id;
            requisitoDeUsuario.Valor = area.Nome;
            await _servicoRequisitoUsuario.CadastrarAsync(requisitoDeUsuario);
        }

        private async Task<PaginatedList<RequisitoDeUsuario>> ObterPaginaRequisitoUsuario(int PageNumber, string idUsuario, UnitOfWork unitOfWork = null)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            List<RequisitoDeUsuario> ListaRequisitos = await ObterRequisitoUsuarioComRequisito(unitOfWork,idUsuario);
            PaginatedList<RequisitoDeUsuario> Requisitos = PaginatedList<RequisitoDeUsuario>.Create(ListaRequisitos.AsQueryable(), PageNumber, 5);
            return Requisitos;
        }

        private async Task<List<RequisitoDeUsuario>> ObterRequisitoUsuarioComRequisito(UnitOfWork unitOfWork,string idUsuario)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            List<RequisitoDeUsuario> ListaRequisitos = await _servicoRequisitoUsuario.BuscarRequisitosDoUsuario(idUsuario);
            foreach(RequisitoDeUsuario rdu in ListaRequisitos)
            {
                rdu.IdRequisitoNavigation = await _servicoRequisito.BuscarPorId(rdu.IdRequisito);
            }
            return ListaRequisitos;
        }

        [HttpGet]
        public async Task<ActionResult> ObterPaginaRequisitoUsuario(int PageNumber, string idUsuario)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            List<RequisitoDeUsuario> ListaRequisitos = await ObterRequisitoUsuarioComRequisito(unitOfWork, idUsuario);
            PaginatedList<RequisitoDeUsuario> Requisitos = PaginatedList<RequisitoDeUsuario>.Create(ListaRequisitos.AsQueryable(), PageNumber, 5);
            return PartialView("_UsuarioRequisitoTable", Requisitos);
        }
    }
}