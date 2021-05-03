using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Acesso.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using ADE.Apresentacao.Areas.Shared;
using ADE.Infra.Data.UOW;
using ADE.Utilidades.EmailMarkup;
using static ADE.Dominio.Models.Enums.EnumEmailTemplate;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Enums;
using Assistente_de_Estagio.Data;
using ADE.Dominio.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ADE.Apresentacao.Areas.Acesso.Controllers
{
    public class AccountController : BaseController
    {
        static UnitOfWork unitOfWork;
        private IHostingEnvironment _hostingEnvironment;
        private AuthMessageSender _emailSender;
        
        private TemplatePathHelper TemplatePathHelper { get; set; }
        private SignInManager<UsuarioADE> signInManager;
        private static ServicoRequisito _servicoRequisito;
        private ServicoInstituicao _servicoInstituicao;
        private ServicoCurso _servicoCurso;
        private ServicoAreaEstagioCurso _servicoAreaEstagio;
        private ServicoRequisitoUsuario _servicoRequisitosDeUsuario;
        public AccountController(UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager,
            IHostingEnvironment hostingEnvironment,
            AuthMessageSender emailSender,
            ApplicationDbContext context
            ) :base(new UnitOfWork(context), userManager, signInManager, new ServicoRequisitoUsuario(ref unitOfWork))
        {
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
            this.signInManager = signInManager;
            unitOfWork = new UnitOfWork(context);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoAreaEstagio = new ServicoAreaEstagioCurso(ref unitOfWork);
            _servicoRequisito = new ServicoRequisito(ref unitOfWork);
            _servicoRequisitosDeUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            TemplatePathHelper = new TemplatePathHelper(hostingEnvironment.WebRootPath);
        }

        [HttpGet]
        [ActionName ("Index")]
        public async Task<IActionResult> Index(string ReturnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return LocalRedirect(ReturnUrl);
                return View("Interstitial", "/Principal/UserHome/Index");
            }
            else
            {
                return View("Login", await ParseLoginView());
            }
        }

        private async Task<LoginViewModel> ParseLoginView()
        {
            LoginViewModel LVM = new LoginViewModel();
            try
            {
                LVM.ExternalLogins = (await ObterAutenticacoesExternas()).ToList();
                return LVM;
            }
            catch(Exception)
            {
                LVM.ExternalLogins = new List<AuthenticationScheme>();
                return LVM;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, bool jsonRQ = false)
        {
            int errorCount = 0;
            try
            {
                ViewData["LoginEmail"] = model.Email;
                if (NullLoginParameters(model))
                {
                    if (jsonRQ)
                            return Json(new { Erro = "Os campos de login não podem estar vazios." });
                 
                    ModelState.AddModelError("Falha", "Os campos de login não podem estar vazios.");

                    return View("Login", await ParseLoginView());
                }

                else if (!ModelState.IsValid)
                {
                    if (jsonRQ)
                        return Json(new { Erro = "Confira se os dados informados estão corretos" });

                    ModelState.AddModelError("Falha", "Confira se os dados informados estão corretos");

                    return View("Login", await ParseLoginView());
                }

                else
                {
                    UsuarioADE usuario = await ObterUsuarioPorEmail(model.Email);
                    if (UsuarioValido(usuario))
                    {
                        var result = await LogarUsuario(usuario, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            await LogarUsuario(usuario, model.Email);

                            if (jsonRQ)
                                return Json(new { Sucesso = "Logado com sucesso" });

                            return View("Interstitial", "/Principal/UserHome/Index");
                        }
                        else
                        {
                            errorCount++;
                            if (jsonRQ)
                                return Json(new { Erro = $"Verifique o e-mail e senha inseridos" });
                            ModelState.AddModelError("Falha", "Verifique o e-mail e senha inseridos");
                        }
                    }
                    else if (!usuario.EmailConfirmed)
                    {
                        ModelState.AddModelError("Falha", $"Falha ao autenticar o usuário, você confirmou o registro por E-mail?");
                        ViewBag.Retorno = $"<form method='post' action='{Url.Action("EnviarEmailDeConfirmacaoDeRegistroLogin", "Account")}'><input type='hidden' name='Email' value='{usuario.Email}'/> <button type='submit' class='text-info''>Falha ao autenticar o usuário: Reenviar e-mail de confirmação</button>";

                        if (jsonRQ)
                            return Json(new { Erro = $"<form method='post' action='{Url.Action("EnviarEmailDeConfirmacaoDeRegistroLogin", "Account")}'><input type='hidden' name='Email' value='{usuario.Email}'/> <button type='submit' class='text-info''>Falha ao autenticar o usuário: Reenviar e-mail de confirmação</button>" });
                    }
                }
            }
            catch(Exception ex)
            {
                if(errorCount == 0)
                    ModelState.AddModelError("Falha", "Verifique a combinação E-mail e Senha");
                await LogError(ex.Message, ex.Source, EnumTipoLog.Login);
            }
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
            );
            if (jsonRQ)
            {
                StringBuilder Erros = new StringBuilder();
                foreach (var entry in ModelState)
                {
                    foreach (ModelError erro in entry.Value.Errors)
                    {
                        Erros.AppendLine(erro.ErrorMessage);
                    }
                }
                return Json(new { Erro = Erros.ToString() });
            }
            return View("Login", await ParseLoginView());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginExterno(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Link("/Acesso/Account/ExternalLogin", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        #region [LoginErrors]

        private bool NullLoginParameters(LoginViewModel model)
        {
            return model.Email == null || model.Password == null ? true : false;
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(string RegistrarEmail, string RegistrarPassword, bool json = true)
        {
            try
            {
                ViewData["RegistroEmail"] = RegistrarEmail;
                if (ValidarEmail(RegistrarEmail))
                {
                    var usuario = new UsuarioADE
                    {
                        UserName = RegistrarEmail,
                        Email = RegistrarEmail,
                        PasswordHash = RegistrarPassword,
                        IdCurso = 0,
                        Logo = IconPadrao(_hostingEnvironment)
                    };

                    var result = await RegistrarUsuario(usuario);

                    if (result.Succeeded)
                    {
                        await EnviarEmailDeConfirmacaoDeRegistro(usuario);
                        await LogarUsuario(usuario, RegistrarEmail);

                        await ManterLogDeRegistro(usuario);

                        if (json)
                            return Json(new { Sucesso = "Sucesso ao registrar" });

                        return View("Interstitial", "/Principal/UserHome/Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email inválido para cadastro");
                }
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            if (json)
            {
                StringBuilder Erros = new StringBuilder();
                foreach(var entry in ModelState)
                {
                    foreach(ModelError erro in entry.Value.Errors) {
                        Erros.Append(erro.ErrorMessage);
                    }
                }
                return Json(new { Erro = Erros.ToString() });
            }

            return View("Login", await ParseLoginView());
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarLogin()
        {
            try
            {
                ServicoLogins _servicoLogins = new ServicoLogins(ref unitOfWork);
                UsuarioADE usuario = await ObterUsuarioLogado();
                Logins login = new Logins(usuario.Id, DateTime.Now);
                await _servicoLogins.CadastrarAsync(usuario, login);
                return Json(new { Sucesso = "Sucesso ao atualizar login" });
            }
            catch(Exception ex)
            {
                return Json(new { Erro = "Erro ao atualizar login" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EntrarSemLogin()
        {
           return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CursosParaInstituicao(int IdInstituicao)
        {
            List<Curso> lista = await _servicoCurso.Filtrar(x => x.IdInstituicao == IdInstituicao);
            return PartialView("_CursoSelect", lista);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AreasEstagioParaCurso()
        {
            UsuarioADE usuario = await ObterUsuarioLogado();
            List<AreaEstagioCurso> lista = await _servicoAreaEstagio.Filtrar(x => x.IdCurso == usuario.IdCurso);
            return PartialView("_AreaCursoSelect", lista);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegistroTemporarioMini(int IdFaculdade, int IdCurso)
        {
            try
            {
                Instituicao instituicao = await _servicoInstituicao.BuscarUm(x => x.Identificador == IdFaculdade);
                Curso curso = await _servicoCurso.BuscarUm(x => x.Identificador == IdCurso);
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (usuario.Id != "N/A")
                {
                    usuario.IdCurso = curso.Identificador;
                    usuario.IdInstituicao = instituicao.Identificador;
                    await AtualizarUsuario(usuario);
                    return RedirectToAction("Index", "UserHome", new { Area = "Principal" });
                }
                else
                {
                    string ContagemUsuarios = (await ContarUsuarios()).ToString().PadLeft(2, '0');
                    string NomeTemporario = $"UsuarioTemporario{instituicao.Nome}{ContagemUsuarios}";
                    string EmailTemporario = $"usuario{ContagemUsuarios}@assistentedeestagio.com.br";

                    var NovoUsuario = new UsuarioADE
                    {
                        UserName = NomeTemporario,
                        Email = EmailTemporario,
                        PasswordHash = DateTime.Now.Ticks.ToString(),
                        IdCurso = curso.Identificador,
                        IdInstituicao = instituicao.Identificador,
                        Logo = IconPadrao(_hostingEnvironment)
                    };

                    var result = await RegistrarUsuario(NovoUsuario);

                    if (result.Succeeded)
                    {
                        await LogarUsuario(NovoUsuario, EmailTemporario);
                        await ManterLogDeRegistro(NovoUsuario);
                        return RedirectToAction("Index", "UserHome", new { area = "Principal" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            return View("Login", await ParseLoginView());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegistroTemporario(int IdFaculdade, int IdCurso, string DadosAluno, bool Estagiando, int IdArea)
        {
            try
            {
                Instituicao instituicao = await _servicoInstituicao.BuscarUm(x => x.Identificador == IdFaculdade);
                Curso curso = await _servicoCurso.BuscarUm(x => x.Identificador == IdCurso);
                List<DadosAlunoKV> dadosAluno = JsonConvert.DeserializeObject<List<DadosAlunoKV>>(DadosAluno);
                foreach (DadosAlunoKV dado in dadosAluno)
                {
                    string id = dado.Name.Split(':')[1];
                    dado.Requisito = await _servicoRequisito.BuscarUm(x => x.Bookmark == id);
                }
                UsuarioADE usuario = await ObterUsuarioLogado();
                usuario.Estagiando = Estagiando;
                if (usuario.Id != "N/A")
                {
                    usuario.IdCurso = curso.Identificador;
                    usuario.IdInstituicao = instituicao.Identificador;
                    await _servicoRequisitosDeUsuario.AdicionarRequisitosDeUsuarioAsync(dadosAluno, usuario);
                    await CadastrarAreaEstagioAluno(usuario, IdArea);
                    await AtualizarUsuario(usuario);
                    return RedirectToAction("Index", "UserHome", new { area = "Principal" });
                }
                else
                {
                    string ContagemUsuarios = (await ContarUsuarios()).ToString().PadLeft(2, '0');
                    string NomeTemporario = $"UsuarioTemporario{instituicao.Nome}{ContagemUsuarios}";
                    string EmailTemporario = $"usuario{ContagemUsuarios}@assistentedeestagio.com.br";

                    var NovoUsuario = new UsuarioADE
                    {
                        UserName = NomeTemporario,
                        Email = EmailTemporario,
                        PasswordHash = DateTime.Now.Ticks.ToString(),
                        IdCurso = curso.Identificador,
                        IdInstituicao = instituicao.Identificador,
                        Estagiando = Estagiando,
                        Logo = IconPadrao(_hostingEnvironment)
                    };

                    var result = await RegistrarUsuario(NovoUsuario);

                    if (result.Succeeded)
                    {
                        await LogarUsuario(NovoUsuario, EmailTemporario);
                        await _servicoRequisitosDeUsuario.AdicionarRequisitosDeUsuarioAsync(dadosAluno, NovoUsuario);
                        await CadastrarAreaEstagioAluno(usuario, IdArea);
                        await ManterLogDeRegistro(NovoUsuario);
                        return RedirectToAction("Index", "UserHome", new { area = "Principal" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            return View("Login", await ParseLoginView());
        }

        private async Task CadastrarAreaEstagioAluno(UsuarioADE usuario, int idArea)
        {
            Requisito req = await _servicoRequisito.BuscarUm(x => x.Bookmark == "_AreasEstagio_");
            AreaEstagioCurso area = await _servicoAreaEstagio.BuscarUm(x => x.Identificador == idArea);
            RequisitoDeUsuario requisitoDeUsuario = new RequisitoDeUsuario();
            requisitoDeUsuario.IdRequisito = req.Identificador;
            requisitoDeUsuario.UserId = usuario.Id;
            requisitoDeUsuario.Valor = area.Nome;
            await _servicoRequisitosDeUsuario.CadastrarAsync(requisitoDeUsuario);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegistroTemporarioDireto(string Faculdade, string Curso)
        {
            try
            {
                string ContagemUsuarios = (await ContarUsuarios()).ToString().PadLeft(2, '0');
                string NomeTemporario = $"UsuarioTemporario{Faculdade}{ContagemUsuarios}";
                string EmailTemporario = $"usuario{ContagemUsuarios}@assistentedeestagio.com.br";

                Instituicao instituicao = await _servicoInstituicao.BuscarUm(x => x.Nome == Faculdade || x.Nome.Contains(Faculdade));
                Curso curso = await _servicoCurso.BuscarUm(x => x.NomeCurso == Curso && x.IdInstituicao == instituicao.Identificador || x.Sigla == Curso && x.IdInstituicao == instituicao.Identificador);

                var usuario = new UsuarioADE
                {
                    UserName = NomeTemporario,
                    Email = EmailTemporario,
                    PasswordHash = DateTime.Now.Ticks.ToString(),
                    IdCurso = curso.Identificador,
                    IdInstituicao = instituicao.Identificador,
                    Logo = IconPadrao(_hostingEnvironment)
                };

                var result = await RegistrarUsuario(usuario);

                if (result.Succeeded)
                {
                    await LogarUsuario(usuario, EmailTemporario);
                    await ManterLogDeRegistro(usuario);
                    return RedirectToAction("Index", "UserHome", new { area = "Principal" });
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
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            return View("Login", await ParseLoginView());
        }

        private async Task EnviarEmailDeConfirmacaoDeRegistro(UsuarioADE usuario)
        {
            var codigo = await GerarCodigoDeConfirmacao(usuario);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { usuario = usuario.Email, codigo }, protocol: HttpContext.Request.Scheme);
            string EmailMarkup = await FormatarEmailRegistro(callbackUrl, usuario.Email);
            await _emailSender.SendEmailAsync(usuario.Email, "Confirme sua conta", EmailMarkup);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarEmailDeConfirmacaoDeRegistro(string Email)
        {
            try
            {
                var email = await ObterEmailUsuario();
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (Email != email)
                {
                    var setEmailResult = await AtualizarEmail(usuario, Email);
                    if (!setEmailResult.Succeeded)
                    {
                        var userId = ObterIdUsuario();
                        throw new InvalidOperationException($"Um erro inesperado ocorreu ao setar o e-mail para '{userId}'.");
                    }
                }
                var codigo = await GerarCodigoDeConfirmacao(Email);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { usuario = Email, codigo }, protocol: HttpContext.Request.Scheme);
                string EmailMarkup = await FormatarEmailRegistro(callbackUrl, Email);
                await _emailSender.SendEmailAsync(Email, "Confirme sua conta", EmailMarkup);
                ViewBag.Retorno = $"O link de confirmação foi enviado para o e-mail {Email}, o mesmo pode levar alguns minutos para chegar. Caso não encontre, cheque a caixa de Spam ou tente novamente";
                await RefreshSignIn(usuario);
                return Redirect("/Acesso/Account/Manage");
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoSenha);
                ViewBag.Retorno = "Erro enviar e-mail de confirmação de registro";
                ModelState.AddModelError("Falha", "Erro enviar e-mail de confirmação de registro");
            }
            return Redirect("/Acesso/Account/Manage");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPerfil(string NomeUsuario, string EmailUsuario)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                if(!ValidarEmail(EmailUsuario)){
                    throw new Exception("E-mail inválido ao alterar informações pessoais.");
                }
                string AlteracaoEmail = usuario.Email != EmailUsuario ? $"Confirme o seu novo e-mail! <br> Email alterado para {EmailUsuario}" : string.Empty;
                string AlteracaoUserName = usuario.UserName != NomeUsuario ? $"Nome de exibição alterado para {NomeUsuario}" : string.Empty;
                if (usuario.UserName != NomeUsuario)
                {
                    usuario.UserName = NomeUsuario;
                    await AtualizarUsuario(usuario);
                    NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, "Nome alterado com sucesso!", AlteracaoUserName);
                    await CriarNotificacaoIndividual(usuario, notificacao);
                }
                if (ValidarEmail(EmailUsuario))
                {
                    if (usuario.Email != EmailUsuario)
                    {
                        usuario.Email = EmailUsuario;
                        usuario.EmailConfirmed = false;
                        await AtualizarUsuario(usuario);
                        
                        NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, "Tudo pronto, só falta confirmar o seu novo e-mail", AlteracaoEmail);
                        await CriarNotificacaoIndividual(usuario, notificacao);

                        await EnviarEmailDeConfirmacaoDeRegistro(EmailUsuario);
                    }
                }
                else
                {
                    await LogError($"E-mail inválido utilizado ao alterar o perfil - usuario {usuario.ToString()}", "AlterarPerfil", EnumTipoLog.AlteracaoEmailUsuario);
                    ViewBag.RetornoEnvioEmail = "Erro alterar o e-mail do usuário, e-mail inválido";
                    ModelState.AddModelError("Falha", "Erro alterar o e-mail do usuário, e-mail inválido");
                    RedirectToPage("/Acesso/Account/Manage");
                }
                ViewBag.Retorno = $"{AlteracaoUserName} <hr> {AlteracaoEmail}";

                await RefreshSignIn(usuario);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoNomeUsuario);
                ViewBag.RetornoEnvioEmail = "Erro ao alterar informações do usuário";
                ModelState.AddModelError("Falha", "Erro ao alterar informações do usuário");
            }
            return RedirectToPage("/Acesso/Account/Manage");

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EnviarEmailDeConfirmacaoDeRegistroLogin(string Email)
        {
            try
            {
                var codigo = await GerarCodigoDeConfirmacao(Email);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { usuario = Email, codigo }, protocol: HttpContext.Request.Scheme);
                string EmailMarkup = await FormatarEmailRegistro(callbackUrl, Email);
                await _emailSender.SendEmailAsync(Email, "Confirme sua conta", EmailMarkup);
                ViewBag.Retorno = $"O link de confirmação foi enviado para o e-mail {Email}, o mesmo pode levar alguns minutos para chegar.<hr><p> Caso não o encontre, cheque a caixa de Spam ou tente novamente.</p>";
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoSenha);
                ViewBag.Retorno = "Erro ao enviar e-mail de confirmação de registro";
                ModelState.AddModelError("Falha", "Erro enviar e-mail de confirmação de registro");
            }
            return View("Login", await ParseLoginView());
        }

        private async Task ManterLogDeRegistro(UsuarioADE user)
        {
            await SalvarLog("Usuario criou uma conta.", "Acesso", EnumTipoLog.RegistroDeUsuario, TipoEvento.Criacao, user);
        }

        [AllowAnonymous]
        private async Task<string> FormatarEmailRegistro(string callbackUrl, string emailUsuario)
        {
            string EmailTemplatePath = TemplatePathHelper.GetEmailTemplatePath(Registro);
            IEmailMarkup email = new Markup_Email_Confirmacao_Registro(callbackUrl, emailUsuario, EmailTemplatePath);
            return await email.Parse();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string usuario, string codigo)
        {
            if (usuario == null || codigo == null)
            {
                ModelState.AddModelError("Erro", $"Sua tentativa de confirmação foi inválida. Acesse esse endereço : <a href='/Account/ConfirmEmailAnon/?usuario={usuario}'>Clicar</a>, e tente novamente. Caso persista, procure o suporte.");
                ViewBag.Retorno = "Sua tentativa de confirmação foi inválida. Acesse esse endereço : <a href='/Account/Logout'>Logout</a>, e tente novamente. Caso persista, procure o suporte.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                try
                {
                    UsuarioADE usuarioADE = await ObterUsuarioPorEmail(usuario);
                    return await ConfirmarEmail(usuarioADE, codigo);
                }
                catch(Exception ex)
                {
                    await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmailAnon(string usuario)
        {
            try
            {
                UsuarioADE usuario1 = await ObterUsuarioPorEmailOuId(usuario);
                usuario1.EmailConfirmed = true;
                await AtualizarUsuario(usuario1);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erro", $"Acesse esse endereço : <a href='/Account/ConfirmEmailAnon/?usuario={usuario}'>Clicar</a>. Caso persista, procure o suporte.");
                ViewBag.Retorno = "Caso persista, procure o suporte.";
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AceitarTermosCompromisso()
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioLogado();
                if (usuario.Id != "N/A")
                {
                    usuario.AceitouTermos = true;
                    await AtualizarUsuario(usuario);
                    return Json(new  {Sucesso = "Aceitou termos"});
                }
                else
                {
                    usuario = await RegistrarELogarUsuario();
                    return Json(new { Sucesso = "Aceitou termos e foi registrado como usuário temporário. <br>Acesse seu perfil para manter a conta."});
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
            }
            return Json(new { Erro = "Um erro inexperado ocorreu." });
        }

        private async Task<UsuarioADE> RegistrarELogarUsuario()
        {
            string ContagemUsuarios = (await ContarUsuarios()).ToString().PadLeft(2, '0');
            string NomeTemporario = $"UsuarioTemporario{ContagemUsuarios}";
            string EmailTemporario = $"usuario{ContagemUsuarios}@assistentedeestagio.com.br";

            var NovoUsuario = new UsuarioADE
            {
                UserName = NomeTemporario,
                Email = EmailTemporario,
                PasswordHash = DateTime.Now.Ticks.ToString(),
                AceitouTermos = true,
                Logo = IconPadrao(_hostingEnvironment)
            };

            var result = await RegistrarUsuario(NovoUsuario);

            if (result.Succeeded)
            {
                await LogarUsuario(NovoUsuario, EmailTemporario);
                await ManterLogDeRegistro(NovoUsuario);
            }
            else
            {
                throw new Exception("Não foi possível cadastrar o usuário " + result.Errors.First());
            }
            return NovoUsuario;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string reason, string returnUrl = null)
        {
            try
            {
                await base.Logout();
                if (returnUrl != null)
                  return LocalRedirect(returnUrl);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.RegistroDeUsuario);
                ViewBag.Retorno = "Erro no logout";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View("ForgotPassword");

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, bool jsonRQ = true)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioADE usuario = await ObterUsuarioPorEmail(model.Email);
                    if (usuario == null || !(await EmailConfirmado(usuario)))
                    {
                        if (jsonRQ)
                            return Json(new { Erro = "Por favor, confira seu e-mail para resetar sua senha. Caso não tenha recebido a mensagem dentro de 5 minutos, tente novamente - Seu e-mail pode não ter sido confirmado." });
                       
                        ViewBag.Retorno = "Por favor, confira seu e-mail para resetar sua senha. Caso não tenha recebido a mensagem dentro de 5 minutos, tente novamente - Seu e-mail pode não ter sido confirmado."; // Don't reveal that the user does not exist or is not confirmed
                        return View(model);
                    }
                    await EnviarEmailDeRecuperacaoDeSenha(usuario);
                    await RegistrarEnvioDeRecuperacaoDeSenha(usuario);
                    
                    if (jsonRQ)
                        return Json(new { Sucesso = "Por favor, confira seu e-mail para resetar sua senha. Caso não tenha recebido a mensagem dentro de 5 minutos, tente novamente." });
                    
                    ViewBag.Retorno = "Por favor, confira seu e-mail para resetar sua senha. Caso não tenha recebido a mensagem dentro de 5 minutos, tente novamente.";
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoSenha);

                if (jsonRQ)
                    return Json(new { Erro = $"Um erro ocorreu: {ex.Message}, -Contate o suporte para mais informações" });

                ViewBag.Retorno = $"Um erro ocorreu: {ex.Message}, -Contate o suporte para mais informações";
            }
            return View(model);
        }
        
        private async Task EnviarEmailDeRecuperacaoDeSenha(UsuarioADE usuario)
        {
            var codigo = await GerarTokenResetDeSenha(usuario);
            string handle = Guid.NewGuid().ToString();
            TempData[handle] = codigo;

            var callbackUrl = Url.Action(
                "ResetPassword", "Account",
                values: new { token = handle, email = usuario.Email },
                protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(usuario.Email, "Resetar senha (ADE)", await FormatarEmailResetSenha(callbackUrl, usuario.Email));
        }

        private async Task<string> FormatarEmailResetSenha(string callbackUrl, string emailUsuario)
        {
            string EmailTemplatePath = TemplatePathHelper.GetEmailTemplatePath(TrocaSenha);
            string LoginPath = Url.Action("Index", "Account");
            IEmailMarkup email = new Markup_Email_Reset_Senha(callbackUrl, emailUsuario, EmailTemplatePath, LoginPath);
            return await email.Parse();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Token = token;
            model.Email = email;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, bool jsonRQ = false)
        {
            try
            {
                UsuarioADE usuario = await ObterUsuarioPorEmail(model.Email);
                if (TempData[model.Token] != null)
                {
                    IdentityResult result = await ResetarSenha(usuario, (string)TempData[model.Token], model.Password);
                    if (result.Succeeded)
                    {
                        if (jsonRQ)
                            return Json(new { Sucesso = "Sucesso ao resetar senha!" });

                        ViewBag.Retorno = "Sucesso ao resetar senha!";
                        return View(model);
                    }
                    else
                    {
                        if (jsonRQ)
                            return Json(new { Erro = "Erro ao resetar senha!" });
                        ModelState.AddModelError("Falha", result.Errors.FirstOrDefault().Description);
                        ViewBag.Retorno = "Erro ao resetar senha!";
                        return View(model);
                    }
                }
                
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.AlteracaoSenha);
                if (jsonRQ)
                    return Json(new { Error = "Erro ao resetar senha!" });

                ViewBag.Retorno = "Erro ao resetar senha!";
                ModelState.AddModelError("Falha", ex.Message);
            }
            return View("ResetPassword",model);
        }

        private async Task RegistrarEnvioDeRecuperacaoDeSenha(UsuarioADE usuario)
        {
            await SalvarLog("Usuario requisitou reset de senha.", "Acesso", EnumTipoLog.AlteracaoSenha, TipoEvento.Alteracao, usuario);
        }

        private byte[] IconPadrao(IHostingEnvironment env)
        {
            byte[] Logo;
            string ImagePath;
            if (env.IsDevelopment())
            {
                ImagePath = Path.Combine(env.WebRootPath.Replace("bin\\Debug\\netcoreapp2.2\\", ""), "Images", "xxxhdpi.png");
            }
            else
            {
                ImagePath = Path.Combine(env.WebRootPath, "Images", "xxxhdpi.png");
            }
            using (Stream fs = new FileStream(ImagePath, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Logo = ms.ToArray();
                }
            }
            return Logo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarrouselImageUrl()
        {
            try
            {
                DirectoryInfo DiretorioDeImagens = new DirectoryInfo(Path.Combine(_hostingEnvironment.WebRootPath,"Images/Backgrounds"));
                FileInfo[] Arquivos = DiretorioDeImagens.GetFiles();
                List<Imagem> Imagens = new List<Imagem>();
                foreach (FileInfo arquivo in Arquivos)
                {
                    Imagem Imagem = new Imagem(arquivo.Name, $"/Images/Backgrounds/{arquivo.Name}");
                    Imagens.Add(Imagem);
                }
                var ImagensJson = JsonConvert.SerializeObject(Imagens);
                return Json(ImagensJson);
            }
            catch(Exception ex)
            {
                await LogError("Erro ao obter imagens de fundo da tela de login -"+ex.Message, ex.Source, EnumTipoLog.AlteracaoSenha);
                return Json("{\"Erro\": \"Erro ao obter imagens de fundo da tela de login\"}");
            }
        }
    }
}