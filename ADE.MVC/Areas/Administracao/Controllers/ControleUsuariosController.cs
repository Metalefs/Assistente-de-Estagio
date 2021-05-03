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
using ADE.Utilidades.EmailMarkup;
using ADE.Utilidades.Extensions;
using Assistente_de_Estagio.Areas.Administracao.Models;
using Assistente_de_Estagio.Areas.Shared;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static ADE.Dominio.Models.Enums.EnumEmailTemplate;

namespace ADE.Apresentacao.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Admin,CriadorConteudo")]
    [Area("Administracao")]
    public class ControleUsuariosController : BaseController
    {
        static UnitOfWork unitOfWork;
        readonly ApplicationDbContext context;
        private readonly AuthMessageSender _emailSender;
        private readonly ServicoLogAcoesEspeciais _servicoLogAcoesEspeciais;
        private readonly ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private readonly ServicoCurso _servicoCurso;
        private readonly ServicoAlteracaoEntidadesSistema _servicoAlteracoes;

        private TemplatePathHelper TemplatePathHelper { get; set; }

        public ControleUsuariosController(
            IHostingEnvironment hostingEnvironment,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            AuthMessageSender emailSender,
            ApplicationDbContext context
            ) : base (unitOfWork = new UnitOfWork(context), userManager, signInManager, new ServicoRequisitoUsuario(ref unitOfWork))
        {
            this.context = context;
            this._emailSender = emailSender;
            this._servicoLogAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
            this._servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            this._servicoAlteracoes = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            this._servicoCurso = new ServicoCurso(ref unitOfWork);
            TemplatePathHelper = new TemplatePathHelper(hostingEnvironment.WebRootPath);
        }
            
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAdministrador(RegistroAdmin registro)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", registro);
                }
                UsuarioADE usuarioCriado = new UsuarioADE()
                {
                    UserName = registro.Nome,
                    Email = registro.Email,
                    PasswordHash = GenerateRandomPassword(),
                    IdCurso = 0
                };
                UsuarioADE agente = await ObterUsuarioLogado();
                var result = await RegistrarUsuarioAdministrador(agente, usuarioCriado, registro.Funcao, registro.Existente);

                if (result.Succeeded)
                {
                    var codigo = await GerarCodigoDeConfirmacao(usuarioCriado);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { usuario = usuarioCriado, codigo }, protocol: HttpContext.Request.Scheme);
                    string EmailMarkup = await FormatarEmailRegistro(callbackUrl, usuarioCriado.Email, usuarioCriado.PasswordHash);
                    await _emailSender.SendEmailAsync(registro.Email, "ADE - Um perfil foi criado para você", EmailMarkup);
                    await ConfirmarEmail(usuarioCriado, codigo);
                    ViewBag.Retorno = $"Usuário {registro.Funcao} - {usuarioCriado.Email} criado com sucesso";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.RegistroDeAdministrador);
            }
            return View("Index", registro);
        }

        [HttpGet]
        public async Task<IActionResult> AcaoDeUsuario(string EmailOrId = null)
        {
            try
            {
                return View(await ParseAcaoUsuario(EmailOrId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter os dados do úsuario");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UsuariosAdministradores(int pageNumber = 1)
        {
            try
            {
                List<string> Funcoes = new List<string>(){ EnumTipoUsuario.Admin.GetDescription(), EnumTipoUsuario.CriadorConteudo.GetDescription() };
                List<List<UsuarioADE>> Lista = await ObterUsuariosPorFuncao(Funcoes);
                PaginatedList<UsuarioADE> Admins = PaginatedList<UsuarioADE>.Create(Lista.First().AsQueryable(), pageNumber, 5);
                PaginatedList<UsuarioADE> CriadoresConteudo = PaginatedList<UsuarioADE>.Create(Lista.Last().AsQueryable(), pageNumber, 5);
                UsuariosAdministradoresViewmodel model = new UsuariosAdministradoresViewmodel
                {
                    Admins = Admins,
                    CriadoresConteudo = CriadoresConteudo
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter os dados do úsuario");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarAlteracoesDeAdministrador(string userName, int pageNumber = 1)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioPorEmail(userName);
                AlteracoesAdministradorViewModel model = new AlteracoesAdministradorViewModel
                {
                    Usuario = await ObterUsuarioPorEmail(userName)
                };
                List<AlteracaoEntidadesSistema> Alteracoes = await _servicoAlteracoes.Filtrar(x => x.IdAutor == usuario.Id);
                PaginatedList<AlteracaoEntidadesSistema> PaginaAlteracoes = PaginatedList<AlteracaoEntidadesSistema>.Create(Alteracoes.AsQueryable(), pageNumber, 10);
                model.Alteracoes = PaginaAlteracoes;
                if (model.Alteracoes.Count == 0)
                {
                    List<AlteracaoEntidadesSistema> Fallback = new List<AlteracaoEntidadesSistema> { new Dominio.Models.AlteracaoEntidadesSistema() { Autor = new UsuarioADE() { Id = "N/A", UserName = "N/A" }, MensagemAlteracao = "N/A", Entidade = EnumEntidadesSistema.Syslogs } };
                    model.Alteracoes = PaginatedList<AlteracaoEntidadesSistema>.Create(Fallback.AsQueryable(), pageNumber, 10); 
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Falha", "Erro ao obter os dados do úsuario");
                await LogError(ex.Message, ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                AlteracoesAdministradorViewModel model = new AlteracoesAdministradorViewModel();
                List <AlteracaoEntidadesSistema> Fallback = new List<AlteracaoEntidadesSistema> { new Dominio.Models.AlteracaoEntidadesSistema() { Autor = new UsuarioADE() { Id = "N/A", UserName = "N/A" }, MensagemAlteracao = "N/A", Entidade = EnumEntidadesSistema.Syslogs } };
                model.Alteracoes = PaginatedList<AlteracaoEntidadesSistema>.Create(Fallback.AsQueryable(), pageNumber, 10);
                model.Usuario = await ObterUsuarioLogado();
                return View("VisualizarAlteracoesDeAdministrador", model);
            }
        }

        private async Task<AcaoUsuarioViewModel> ParseAcaoUsuario(string EmailOrId = null)
        {
            UsuarioADE usuario = EmailOrId == null ? await ObterUsuarioLogado() : await ObterUsuarioPorEmailOuId(EmailOrId);
            AcaoUsuarioViewModel model = new AcaoUsuarioViewModel()
            {
                LogAcaoUsuario = await ObterPaginaLogAcaoUsuario(1, EmailOrId),
                Usuario = usuario,
                CursoUsuario = await _servicoCurso.BuscarPorId(usuario.IdCurso),
                QuantidadeDocumentosGerados = await _servicoHistoricoGeracaoDocumento.CountByFilter(x => x.IdUsuario == EmailOrId || x.IdUsuarioNavigation.Email == EmailOrId),
                AutorizacaoUsuario = await ObterAutorizacaoUsuario(usuario)
            };
            return model;
        }

        private async Task<PaginatedList<LogAcoesEspeciais>> ObterPaginaLogAcaoUsuario(int PageNumber, string UserEmailOrId)
        {
            List<LogAcoesEspeciais> ListaLogAcoes = await _servicoLogAcoesEspeciais.Filtrar(x => x.IdUsuario == UserEmailOrId || x.IdUsuarioNavigation.Email == UserEmailOrId);
            PaginatedList<LogAcoesEspeciais> Logs = PaginatedList<LogAcoesEspeciais>.Create(ListaLogAcoes.AsQueryable(), PageNumber, 5);
            return Logs;
        }

        [HttpGet]
        public async Task<ActionResult> ObterPaginaLogAcaoEspecial(int PageNumber, string UserEmailOrId)
        {
            List<LogAcoesEspeciais> ListaLogAcoes = await _servicoLogAcoesEspeciais.Filtrar(x => x.IdUsuario == UserEmailOrId || x.IdUsuarioNavigation.Email == UserEmailOrId);
            PaginatedList<LogAcoesEspeciais> Logs = PaginatedList<LogAcoesEspeciais>.Create(ListaLogAcoes.AsQueryable(), PageNumber, 5);
            return PartialView("_AdmAcaoUsuarioTable", Logs);
        }

        private async Task<string> FormatarEmailRegistro(string callbackUrl, string emailUsuario, string TempPassword)
        {
            string EmailTemplatePath = TemplatePathHelper.GetEmailTemplatePath(RegistroAdministrador);
            IEmailMarkup email = new Markup_Email_Confirmacao_Registro_Admnistrador(callbackUrl, emailUsuario, TempPassword, EmailTemplatePath);
            return await email.Parse();
        }

        private static string GenerateRandomPassword(PasswordOptions opts = null) => DateTime.Now.Ticks.ToString();
    }
}