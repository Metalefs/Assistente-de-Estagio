using Microsoft.AspNetCore.Mvc;
using ADE.Aplicacao.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Assistente_de_Estagio.Areas.Administracao.Models;
using ADE.Dominio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Apresentacao.Areas.Shared;
using Microsoft.AspNetCore.Identity;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Areas.Shared;
using System.Linq;
using Assistente_de_Estagio.Data;
using System;

namespace Assistente_de_Estagio.Controllers
{
    //[RequireHttps]
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class DashboardController : BaseController
    {
        readonly IHostingEnvironment env;
        readonly ApplicationDbContext context;
        private UnitOfWork unitOfWork;
        private ServicoCurso _servicoCurso;
        private ServicoDocumento _servicoDocumento;
        private ServicoRequisito _servicoRequisito;
        private ServicoLogins _loginServices;
        private ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private ServicoLogAcoesEspeciais _servicoLogAcoesEspeciais;
        private ServicoSysLogs _servicoSysLogs;

        public DashboardController(
            IHostingEnvironment _env,
            UserManager<UsuarioADE> userManager,
            ApplicationDbContext context
            )
            : base(new UnitOfWork(context), userManager)
        {
            unitOfWork = new UnitOfWork(context);
            this.context = context;
            env = _env;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                DashboardViewModel dashboardViewModel = await ParseDashboardView();
                return View(dashboardViewModel);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter os dados para dashboard");
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        public async Task<DashboardViewModel> ParseDashboardView()
        { 
            _servicoDocumento = new ServicoDocumento(ref unitOfWork, env);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            _loginServices = new ServicoLogins(ref unitOfWork);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);

            DashboardViewModel dbvm = new DashboardViewModel()
            {
                TotalCursos = await _servicoCurso.Count(),
                TotalDocumentos = await _servicoDocumento.Count(),
                TotalRequisitos = await _servicoRequisito.Count(),
                TotalDownloads = await _servicoHistoricoGeracaoDocumento.Count(),
                TotalUsuarios = await ContarUsuarios(),
                TotalUsuariosLogados = await ContarUsuariosLogados(),
                LogAcoes = await ObterPaginaLogAcaoEspecial(1, unitOfWork),
                ErrorLogs = await ObterPaginaSysLog(1, unitOfWork)
            };
            return dbvm;
        }

        private async Task<PaginatedList<LogAcoesEspeciais>> ObterPaginaLogAcaoEspecial(int PageNumber, UnitOfWork unitOfWork = null)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
                _servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
                List<LogAcoesEspeciais> ListaLogAcoes = await _servicoLogAcoesEspeciais.ListarAsync();
                PaginatedList<LogAcoesEspeciais> Logs = PaginatedList<LogAcoesEspeciais>.Create(ListaLogAcoes.AsQueryable(), PageNumber, 5);
            return Logs;
        }

        [HttpGet]
        public async Task<ActionResult> ObterPaginaLogAcaoEspecial(int PageNumber)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                _servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
                List<LogAcoesEspeciais> ListaLogAcoes = await _servicoLogAcoesEspeciais.ListarAsync();
                PaginatedList<LogAcoesEspeciais> Logs = PaginatedList<LogAcoesEspeciais>.Create(ListaLogAcoes.AsQueryable(), PageNumber, 5);
                return PartialView("_AdmMessageTable", Logs);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter página de logs de ações do sistema");
                ViewBag.Retorno = "Erro ao obter página de logs de ações do sistema";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return PartialView("_AdmMessageTable", PaginatedList<LogAcoesEspeciais>.Create(new List<LogAcoesEspeciais>() { new LogAcoesEspeciais("Erro","Erro",ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno,"Erro")}.AsQueryable(), PageNumber, 5));
            }
        }

        private async Task<PaginatedList<SysLogs>> ObterPaginaSysLog(int PageNumber, UnitOfWork unitOfWork = null)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork(context);
            _servicoSysLogs = new ServicoSysLogs(ref unitOfWork);
            List<SysLogs> ListaSysLogs = await _servicoSysLogs.ListarAsync();
            PaginatedList<SysLogs> Logs = PaginatedList<SysLogs>.Create(ListaSysLogs.AsQueryable(), PageNumber, 5);
            return Logs;
        }

        [HttpGet]
        public async Task<ActionResult> ObterPaginaSysLog(int PageNumber)
        {
            try
            {
                unitOfWork = unitOfWork ?? new UnitOfWork(context);
                _servicoSysLogs = new ServicoSysLogs(ref unitOfWork);
                List<SysLogs> ListaSysLogs = await _servicoSysLogs.ListarAsync();
                PaginatedList<SysLogs> Logs = PaginatedList<SysLogs>.Create(ListaSysLogs.AsQueryable(), PageNumber, 5);
                return PartialView("_AdmSysLogTable", Logs);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter pagina de Logs de erro");
                ViewBag.Retorno = "Erro ao obter página de logs de ações do sistema";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return PartialView("_AdmSysLogTable", PaginatedList<SysLogs>.Create(new List<SysLogs>() { new SysLogs("Erro", "Erro", ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno) }.AsQueryable(), PageNumber, 5));
            }
        }

        public async Task<string> DownloadDocumentosPorCursoJson()
        {
            try
            {
                using (unitOfWork = new UnitOfWork(context))
                {
                    _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
                    List<HistoricoGeracaoDocumento> historicoGeracao = await _servicoHistoricoGeracaoDocumento.ListarAsync();
                    List<HistoricoGeracaoDocumento> HistoricoGeracaoComCurso = await ObterHistoricoGeracaoComCurso(historicoGeracao);
                    IChart Chart = new DownloadDocumentosPorCurso(HistoricoGeracaoComCurso);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(Chart);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter dados do gráfico de Download de Documentos Por Curso");
                ViewBag.Retorno = "Erro ao obter dados do gráfico de Download de Documentos Por Curso";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "{Values: ['1'], Labels: ['Erro']}";
            }
        }

        private async Task<List<HistoricoGeracaoDocumento>> ObterHistoricoGeracaoComCurso(List<HistoricoGeracaoDocumento> historicos)
        {
            _servicoDocumento = new ServicoDocumento(ref unitOfWork, env);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            foreach (HistoricoGeracaoDocumento historico in historicos)
            {
                Documento documento = await _servicoDocumento.BuscarPorId(historico.Documento);
                Curso Curso = await _servicoCurso.BuscarPorId(documento.IdCurso);
                historico.IdDocumentoNavigation = documento;
                historico.IdDocumentoNavigation.IdCursoNavigation = Curso;
            }
            return historicos;
        }

        public async Task<string> QuantidadeDocumentosPorCursoJson()
        {
            try
            {
                _servicoCurso = new ServicoCurso(ref unitOfWork);

                List<Curso> cursos = await _servicoCurso.ListarAsync();
                List<Curso> CursosComDocumento = new List<Curso>();
                foreach (Curso curso in cursos)
                {
                    CursosComDocumento.Add(await this.ObterCursosComDocumento(curso));
                }
                IChart Chart = new QuantidadeDocumentosPorCurso(CursosComDocumento);
                return Newtonsoft.Json.JsonConvert.SerializeObject(Chart);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter dados do gráfico de Quantidade de Documentos Por Curso");
                ViewBag.Retorno = "Erro ao obter dados do gráfico de Quantidade de Documentos Por Curso";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "{Values: ['1'], Labels: ['Erro']}";
            }
        }

        private async Task<Curso> ObterCursosComDocumento(Curso curso)
        {
            _servicoDocumento = new ServicoDocumento(ref unitOfWork, env);
            List<Documento> Documento = await _servicoDocumento.ListarPorCurso(curso.Identificador);
            curso.Documento = Documento;
            return curso;
        }

        public async Task<string> UsuariosPorCursoJson()
        {
            try
            { 
                List<UsuarioADE> Usuarios = await ListarUsuarios();
                List<UsuarioADE> UsuariosComCurso = new List<UsuarioADE>();
                foreach (UsuarioADE usuario in Usuarios)
                {
                    UsuariosComCurso.Add(await this.ObterUsuarioComDadosPessoais(usuario));
                }
                IChart Chart = new UsuariosPorCurso(UsuariosComCurso);
                return Newtonsoft.Json.JsonConvert.SerializeObject(Chart);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter dados do gráfico de Usuarios Por Curso");
                ViewBag.Retorno = "Erro ao obter dados do gráfico de Usuarios Por Curso";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "{Values: ['1'], Labels: ['Erro']}";
            }
        }

        private new async Task<UsuarioADE> ObterUsuarioComDadosPessoais(UsuarioADE usuario)
        {
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);

            List<HistoricoGeracaoDocumento> ListaHistoricoGeracao = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id);
            Curso curso = await _servicoCurso.BuscarPorId(usuario.IdCurso);
            usuario.IdCursoNavigation = curso;
            usuario.HistoricoGeracaoDocumento = ListaHistoricoGeracao;
            return usuario;
        }

        public async Task<string> TrafegoSiteJson()
        {
            try
            { 
                _loginServices = new ServicoLogins(ref unitOfWork);
                List<Logins> Logins = await _loginServices.ListarAsync();
                IChart Chart = new TrafegoSite(Logins);
                return Newtonsoft.Json.JsonConvert.SerializeObject(Chart);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter dados do gráfico tráfego do site");
                ViewBag.Retorno = "Erro ao obter dados do gráfico tráfego do site";
                await LogError(ex.Message, ex.Source, ADE.Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "{Values: ['1'], Labels: ['Erro']}";
            }
        }
    }
}
