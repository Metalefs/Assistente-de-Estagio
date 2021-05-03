using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using ADE.Utilidades.Constants;
using ADE.Utilidades.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ADE.Apresentacao.Areas.Shared
{
    public partial class BaseController : Controller
    {
        private ServicoUsuario ServicoUsuario;
        private UnitOfWork unitOfWork;
        private ServicoLogAcoesEspeciais ServicoLog;
        private ServicoNotificacaoIndividual ServicoNotificacaoIndividual;
        private ServicoSysLogs ServicoLogErros;
        private SignInManager<UsuarioADE> SignInManager;

        public BaseController(
            UnitOfWork _unitOfWork,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager = null,
            ServicoRequisitoUsuario servicoRequisitoUsuario = null,
            ServicoHistoricoGeracaoDocumento servicoHistoricoGeracaoDocumento = null
        )
        {
            unitOfWork = _unitOfWork;
            SignInManager = signInManager;
            ServicoUsuario = new ServicoUsuario(
                _unitOfWork,
                userManager,
                signInManager,
                servicoRequisitoUsuario,
                servicoHistoricoGeracaoDocumento
                );
            ServicoNotificacaoIndividual = new ServicoNotificacaoIndividual(ref unitOfWork);
        }
        [HttpGet]
        public async Task<IActionResult> ObterPagina(string view)
        {
            return PartialView(view);
        }
        public async Task CriarNotificacaoIndividual(UsuarioADE usuario, NotificacaoIndividual notificacao)
        {
            await ServicoNotificacaoIndividual.CadastrarAsync(usuario,notificacao);
        }
        
        public async Task<RequisitosBasicosCabecalho> ObterRequisitosBasicosUsuario(UsuarioADE usuario)
        {
            ServicoRequisitoUsuario _servicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork); 
            ServicoRequisito _servicoRequisito = new ServicoRequisito(ref unitOfWork); 
            List<RequisitoDeUsuario> RDU = await _servicoRequisitoUsuario.BuscarRequisitosDoUsuario(usuario.Id);

            foreach (RequisitoDeUsuario req in RDU)
            {
                req.IdRequisitoNavigation = await _servicoRequisito.BuscarPorId(req.IdRequisito);
            }

            RequisitosBasicosCabecalho requisitosFicha = new RequisitosBasicosCabecalho();
            requisitosFicha.NomeAluno = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.NomeAluno_) != null ? 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.NomeAluno_).Valor 
                : "";
            requisitosFicha.RA = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.RegistroAcademico_) != null ?
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.RegistroAcademico_).Valor
                : "";
            requisitosFicha.Turma = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.TurmaAluno_) != null ? 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.TurmaAluno_).Valor
                : "";
            requisitosFicha.Carga_Horaria_Exigida = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.TotalCargaHorariaEstagio_) != null ?
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.TotalCargaHorariaEstagio_).Valor
                : "";
            requisitosFicha.Nome_Instituicao = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.NomeEmpresa_) != null ?
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.NomeEmpresa_).Valor
                : "";
            requisitosFicha.AreaEstagio = 
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.AreasEstagio_) != null ?
                RDU.FirstOrDefault(x => x.IdRequisitoNavigation.Bookmark == NomesRequisitos.AreasEstagio_).Valor
                : "";
            return requisitosFicha;
        }

        public async Task SalvarLog(string Mensagem, string LocalAcao, EnumTipoLog tipoLog, TipoEvento Acao, UsuarioADE usuario = null)
        {
            ServicoLog = new ServicoLogAcoesEspeciais(ref unitOfWork);
            try
            {
                if (usuario == null)
                    usuario = await ObterUsuarioLogado();
                await ServicoLog.Log(usuario,Mensagem, LocalAcao,tipoLog,Acao);
            }
            catch (NullReferenceException ex)
            {
                usuario = new UsuarioADE
                {
                    Id = "Erro ao obter usuário logado"
                };
                await LogError("NullReferenceException: " + Mensagem + "\n" + ex.Message, "SalvarLog", EnumTipoLog.ErroInterno);
                await ServicoLog.Log(usuario, Mensagem, LocalAcao, tipoLog, Acao);
            }
            await unitOfWork.Commit();
        }
        public async Task <IList<string>> ObterAutorizacaoUsuario(UsuarioADE usuario) => await ServicoUsuario.ObterAutorizacaoUsuario(usuario);
        public async Task<IList<UsuarioADE>> ObterUsuariosPorFuncao(string roleName) => await ServicoUsuario.ObterUsuariosPorFuncao(roleName);
        public async Task<List<List<UsuarioADE>>> ObterUsuariosPorFuncao(List<string> roleNames)
        {
            List<List<UsuarioADE>> usuarios = new List<List<UsuarioADE>>();
            foreach (string name in roleNames)
            {
                IList<UsuarioADE> Iusuarios = (await ServicoUsuario.ObterUsuariosPorFuncao(name));
                usuarios.Add(Iusuarios.ToList());
            }
            return usuarios;
        }
        public async Task<IEnumerable<AuthenticationScheme>> ObterAutenticacoesExternas()
        {
            try
            {
                return await SignInManager.GetExternalAuthenticationSchemesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool UsuarioValido() => User != null && User.Identity.IsAuthenticated;
        public bool UsuarioValido(UsuarioADE usuario) => usuario != null && usuario.EmailConfirmed && usuario.DataHoraExclusao == null;
        public async Task<IdentityResult> RegistrarUsuario(UsuarioADE usuarioAgente)
        {
            IdentityResult result = await ServicoUsuario.CadastrarAsync(usuarioAgente, usuarioAgente.PasswordHash);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuário {usuarioAgente.ToString()} foi cadastrado no sistema", "BaseController RegistrarUsuario", EnumTipoLog.RegistroDeUsuario, TipoEvento.Criacao, usuarioAgente);
            }
            return result;
        }
        public async Task<IdentityResult> RegistrarUsuarioAdministrador(UsuarioADE usuarioAgente, UsuarioADE usuarioCriado, EnumTipoUsuario funcao, bool existente)
        {
            IdentityResult result = await ServicoUsuario.CadastrarAdministradorAsync(usuarioCriado, usuarioCriado.PasswordHash, existente, funcao);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuário {usuarioAgente.ToString()} CADASTROU o usuário {usuarioCriado.ToString()} no sistema como {funcao}", "BaseController RegistrarUsuarioAdministrador", EnumTipoLog.AlteracaoUsuario, TipoEvento.Alteracao, usuarioAgente);
            }
            return result;
        }
        public async Task<List<UsuarioADE>> ListarUsuarios() => await ServicoUsuario.Listar();
        public async Task LogarUsuario(UsuarioADE user, string NameClaim)
        {
            ClaimsPrincipal UserClaims = await ServicoUsuario.LogarUsuario(user, NameClaim);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(UserClaims));
        }
        public async Task<SignInResult> LogarUsuario(UsuarioADE user, string Password, bool RememberMe, bool log) => await ServicoUsuario.LogarUsuario(user, Password, RememberMe, log);
        public async Task<bool> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
                );
                UsuarioADE usuario = await ObterUsuarioLogado();
                await ServicoUsuario.Logout(usuario);
                return true;
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            
        }
        public async Task RefreshSignIn(UsuarioADE usuario) => await ServicoUsuario.RefreshSignIn(usuario);
        public async Task<IdentityResult> AtualizarUsuario(UsuarioADE usuarioAgente, UsuarioADE usuarioCriado = null)
        {
            IdentityResult result = await ServicoUsuario.AtualizarAsync(usuarioAgente, usuarioCriado);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuario {usuarioAgente.ToString()} atualizou informações pessoais", "BaseController AtualizarAsync", EnumTipoLog.AlteracaoUsuario, TipoEvento.Alteracao, usuarioAgente);
            }
            return result;
        }
        public async Task<IdentityResult> AtualizarEmail(UsuarioADE usuario, string Email)
        {
            IdentityResult result = await ServicoUsuario.AtualizarEmail(usuario, Email);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuario {usuario.ToString()} seu e-mail", "BaseController AtualizarEmail", EnumTipoLog.AlteracaoUsuario, TipoEvento.Alteracao, usuario);
            }
            return result;
        }
        public async Task<IdentityResult> RemoverUsuario(UsuarioADE usuarioAgente, UsuarioADE usuarioCriado = null)
        {
            IdentityResult result = await ServicoUsuario.RemoverAsync(usuarioAgente, usuarioCriado);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuario {usuarioAgente.ToString()} removeu seus dados pessoais", "BaseController RemoverUsuario", EnumTipoLog.DelecaoUsuario, TipoEvento.Alteracao);
            }
            return result;
        }
        public async Task<UsuarioADE> ObterUsuarioLogado()
        {
            try
            {
                UsuarioADE usuario = await ServicoUsuario.ObterUsuarioLogado(User);    
                if(usuario == null)
                {
                    return new UsuarioADE() { Id = "N/A", UserName = "N/A" };
                }
                return usuario;
            }
            catch(Exception ex)
            {
                return new UsuarioADE() { Id = "N/A", UserName = "N/A" };
            }
        }
        public async Task<UsuarioADE> ObterUsuarioComDadosPessoais()
        {
            try
            {
                UsuarioADE usuario = await ServicoUsuario.ObterUsuarioComDadosPessoais(User);
                if (usuario == null)
                {
                    return new UsuarioADE() { Id = "N/A", UserName = "N/A" };
                }
                return usuario;
            }
            catch(Exception ex)
            {
                return new UsuarioADE() { Id = "N/A", UserName = "N/A" };
            }
        }
        public async Task<UsuarioADE> ObterUsuarioComDadosPessoais(UsuarioADE usuario) => await ServicoUsuario.ObterUsuarioComDadosPessoais(usuario);
        public string ObterIdUsuario() => ServicoUsuario.ObterIdUsuario(User);
        public async Task<string> ObterEmailUsuario() => await ServicoUsuario.ObterEmailUsuario(await ObterUsuarioLogado());
        public async Task<string> ObterIdUsuario(UsuarioADE usuario) => await ServicoUsuario.ObterIdUsuario(usuario);
        public async Task<UsuarioADE> ObterUsuarioPorEmail(string email) => await ServicoUsuario.ObterUsuarioPorEmail(email);
        public async Task<UsuarioADE> ObterUsuarioPorEmailOuId(string EmailOuId)
        {
            try
            {
                return await ServicoUsuario.ObterUsuarioPorEmailOuId(EmailOuId);
            }
            catch(Exception ex)
            {
                await LogError(ex.Message, ex.Source, EnumTipoLog.Outro);
            }
            return new UsuarioADE() { Email = "Usuário não encontrado", Id = "Usuário não encontrado", UserName = "Usuário não encontrado" };
        }
        public async Task<string> GerarCodigoDeConfirmacao(UsuarioADE usuario) => await ServicoUsuario.GerarCodigoDeConfirmacao(usuario);
        [AllowAnonymous]
        public async Task<string> GerarCodigoDeConfirmacao(string Email) => await ServicoUsuario.GerarCodigoDeConfirmacao(Email);
        public async Task<string> GerarTokenResetDeSenha(UsuarioADE usuario) => await ServicoUsuario.GerarTokenResetDeSenha(usuario);
        public async Task<ActionResult> ConfirmarEmail(UsuarioADE usuario, string code)
        {
            try
            {
                IdentityResult result = await ServicoUsuario.ConfirmarEmail(usuario, code);
                if (result.Succeeded)
                {
                    await SalvarLog($"Usuario {usuario.ToString()} confirmou seu cadastro por e-mail", "BaseController ConfirmarEmail",EnumTipoLog.RegistroDeUsuario, TipoEvento.Criacao, usuario);
                    await LogarUsuario(usuario, usuario.Email);
                    return View("ConfirmEmail");
                }
                else
                {
                    List<IdentityError> ErrorList = result.Errors.ToList();
                    if (ErrorList.Count > 0)
                    {
                        foreach (IdentityError error in ErrorList)
                        { 
                           await LogError($"Erro ao confirmar o email para o usuario: '{usuario.Email}': Erro: {error.Description}", "ConfirmarEmail", EnumTipoLog.ErroInterno);
                           ModelState.AddModelError(error.Code, $"Erro ao confirmar o email para o usuario: '{usuario.Email}': Erro: {error.Description}");
                        }
                    }
                    return RedirectToAction("Index", "Account");
                }
            }
            catch(Exception ex)
            {
                await LogError(ex.Message,ex.Source,EnumTipoLog.RegistroDeUsuario);
                ModelState.AddModelError("Falha", $"Erro ao confirmar o email para o usuario: '{usuario.ToString()}'");
                return RedirectToAction("Index", "Account");
            }
        }
        public async Task<bool> EmailConfirmado(UsuarioADE usuario) => await ServicoUsuario.EmailConfirmado(usuario);
        public async Task<IdentityResult> TrocarSenha(UsuarioADE usuario, string senhaAntiga, string senhaNova) => await ServicoUsuario.TrocarSenha(usuario, senhaAntiga, senhaNova);
        public async Task<IdentityResult> ResetarSenha(UsuarioADE usuario, string Token, string senha) => await ServicoUsuario.ResetarSenha(usuario, Token, senha);
        public async Task<IdentityResult> SetarTipoRecebimentoNotificacao(UsuarioADE usuario, EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao)
        {

            IdentityResult result = await ServicoUsuario.SetarTipoRecebimentoNotificacao(usuario, TipoRecebimentoNotificacao);
            if (result.Succeeded)
            {
                await SalvarLog($"Usuario {usuario.ToString()} removeu seus dados pessoais", "BaseController RemoverUsuario", EnumTipoLog.DelecaoUsuario, TipoEvento.Alteracao);
            }
            return result;
        }
        public async Task<bool> ValidarSenhaDigitada(UsuarioADE usuario, string senha)
        {
            try
            {
                return await ServicoUsuario.ChecarSenhaDigitada(usuario, senha);
            }
            catch (System.ArgumentNullException)
            {
                return false;
            }
        }
        public bool ValidarEmail(string email) => ServicoUsuario.ValidarEmail(email);
        public Task<int> ContarUsuarios() => ServicoUsuario.Count();
        public Task<int> ContarUsuariosLogados() => ServicoUsuario.CountLoggedIn();
        public Task<List<UsuarioADE>> FiltrarUsuarios(Expression<Func<UsuarioADE, bool>> expression) => ServicoUsuario.Filtrar(expression);
        public Task<int> CountUsuarioByInstituicao(int idInstituicao) => ServicoUsuario.CountByInstituicao(idInstituicao);
        public Task<int> CountUsuarioByCurso(int idCurso) => ServicoUsuario.CountByCurso(idCurso);
        public async Task LogError(string mensagem, string localOrigem, EnumTipoLog acaoSistema)
        {
            ServicoLogErros = new ServicoSysLogs(ref unitOfWork);
            SysLogs sysLogs = new SysLogs(mensagem, localOrigem, acaoSistema);
            UsuarioADE usuario = await ObterUsuarioLogado();
            await ServicoLogErros.CadastrarAsync(usuario,sysLogs);
        }
    }
}