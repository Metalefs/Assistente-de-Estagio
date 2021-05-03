using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using ADE.Infra.Data.UOW;
using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Extensions;

namespace ADE.Aplicacao.Services
{
    public class ServicoUsuario : IServicoUsuario, ICountable
    {
        private UnitOfWork unitOfWork;
        private UserManager<UsuarioADE> _userManager;
        private SignInManager<UsuarioADE> _signInManager;
        private ServicoRequisitoUsuario _servicoRequisitoDeUsuario;
        private ServicoHistoricoGeracaoDocumento _servicoHistoricoGeracaoDocumento;
        private ServicoCurso _servicoCurso;
        private ServicoLogins _servicoLogins;
        private ServicoInstituicao _servicoInstituicao;

        public ServicoUsuario(
            UnitOfWork _unitOfWork,
            UserManager<UsuarioADE> userManager,
            SignInManager<UsuarioADE> signInManager = null,
            ServicoRequisitoUsuario servicoRequisitoDeUsuario = null,
            ServicoHistoricoGeracaoDocumento servicoHistoricoGeracaoDocumento = null,
            ServicoCurso servicoCurso = null
        )
        {
            unitOfWork = _unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _servicoRequisitoDeUsuario = servicoRequisitoDeUsuario;
            _servicoHistoricoGeracaoDocumento = servicoHistoricoGeracaoDocumento;
            _servicoCurso = servicoCurso;
        }
        
        public async Task<IdentityResult> CadastrarAsync(UsuarioADE usuario, string password)
        {
            usuario.DataHoraInclusao = DateTime.Now;
            usuario.TipoRecebimentoNotificacao = EnumTipoRecebimentoNotificacao.Geral;
            ServicoRequisito servicoRequisito = new ServicoRequisito(ref unitOfWork);
            try
            {
                List<Requisito> requisito = await servicoRequisito.Filtrar(x=> x.Identificador == 2 || x.NomeRequisito.Contains("Email"));
                RequisitoDeUsuario rdu = new RequisitoDeUsuario(usuario.Id, requisito.FirstOrDefault().Identificador, usuario.Email, usuario);
                _servicoRequisitoDeUsuario = _servicoRequisitoDeUsuario ?? new ServicoRequisitoUsuario(ref unitOfWork);
                await _servicoRequisitoDeUsuario.CadastrarAsync(rdu);

            }catch(Exception ex)
            {            }
            IdentityResult result = await _userManager.CreateAsync(usuario, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, EnumTipoUsuario.Membro.GetDescription());
                return result;
            }
            return result;
        }

        public async Task<IList<string>> ObterAutorizacaoUsuario(UsuarioADE usuario) => await _userManager.GetRolesAsync(usuario);

        public async Task<IList<UsuarioADE>> ObterUsuariosPorFuncao(string roleName) => await _userManager.GetUsersInRoleAsync(roleName);
        
        public async Task<IdentityResult> CadastrarAdministradorAsync(UsuarioADE usuarioCriado, string password, bool existente, EnumTipoUsuario Funcao)
        {
            usuarioCriado.DataHoraInclusao = DateTime.Now;
            usuarioCriado.TipoRecebimentoNotificacao = EnumTipoRecebimentoNotificacao.Geral;
            IdentityResult result;
            if (existente)
            {
                result = await CadastrarAdministradorExistente(usuarioCriado, password, Funcao);
                await AtualizarAsync(usuarioCriado, null);
            }
            else
            {
                result = await _userManager.CreateAsync(usuarioCriado, password);
                if (result.Succeeded)
                {
                    ServicoRequisito servicoRequisito = new ServicoRequisito(ref unitOfWork);
                    Requisito requisito = await servicoRequisito.BuscarUm(x => x.NomeRequisito == "Email");
                    RequisitoDeUsuario rdu = new RequisitoDeUsuario(usuarioCriado.Id, requisito.Identificador, usuarioCriado.Email, usuarioCriado);
                    _servicoRequisitoDeUsuario = _servicoRequisitoDeUsuario ?? new ServicoRequisitoUsuario(ref unitOfWork);
                    await _servicoRequisitoDeUsuario.CadastrarAsync(rdu);
                    await _userManager.AddToRoleAsync(usuarioCriado, Funcao.GetDescription());
                }
            }
            return result;
        }

        public async Task<IdentityResult> CadastrarAdministradorExistente(UsuarioADE usuarioCriado, string password, EnumTipoUsuario Funcao)
        {
            UsuarioADE usuario = await ObterUsuarioPorEmail(usuarioCriado.Email);
            IdentityResult result_existing_user = await _userManager.AddToRoleAsync(usuario, Funcao.GetDescription());
            return result_existing_user;
        }
        public async Task<SignInResult> LogarUsuario(UsuarioADE usuario, string Password, bool RememberMe, bool log)
        {
            usuario.Logado = true;
            await _userManager.UpdateAsync(usuario);
            if (log)
            {
                _servicoLogins = new ServicoLogins(ref unitOfWork);
                Logins login = new Logins(usuario.Id, DateTime.Now);
                await _servicoLogins.CadastrarAsync(usuario, login);
            }
            return await _signInManager.PasswordSignInAsync(usuario.UserName, Password, RememberMe, false);
        }
        public async Task<ClaimsPrincipal> LogarUsuario(UsuarioADE usuario, string NameClaim)
        {
            usuario.Logado = true;
            await _userManager.UpdateAsync(usuario);
            ClaimsPrincipal UserClaims = await _signInManager.CreateUserPrincipalAsync(usuario);
            _servicoLogins = new ServicoLogins(ref unitOfWork);
            Logins login = new Logins(usuario.Id, DateTime.Now);
            await _servicoLogins.CadastrarAsync(usuario, login);
            await _signInManager.SignInAsync(usuario, false);
            return UserClaims;
        }

        public async Task Logout(UsuarioADE usuario)
        {
            usuario.Logado = false;
            await _userManager.UpdateAsync(usuario);
            await _signInManager.SignOutAsync();
            _servicoLogins = new ServicoLogins(ref unitOfWork);
            Logins login = _servicoLogins.BuscarUltimoLoginUsuario(usuario.Id);
            if (login != null)
            {
                login.DataHoraLogout = DateTime.Now;
                await _servicoLogins.DeslogarAsync(usuario, login);
            }
        }

        public async Task<UsuarioADE> ObterUsuarioLogado(ClaimsPrincipal User) => await _userManager.GetUserAsync(User);

        public async Task<UsuarioADE> ObterUsuarioComDadosPessoais(ClaimsPrincipal User)
        {
            UsuarioADE usuario = await _userManager.GetUserAsync(User);
            _servicoRequisitoDeUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            if(usuario != null)
            {
                List<RequisitoDeUsuario> ListaRequisitoUsuario = await _servicoRequisitoDeUsuario.BuscarRequisitosDoUsuario(usuario.Id);
                List<HistoricoGeracaoDocumento> ListaHistoricoGeracao = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id);
                Curso curso = await _servicoCurso.BuscarPorId(usuario.IdCurso);
                Instituicao instituicao = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
                usuario.IdCursoNavigation = curso;
                usuario.HistoricoGeracaoDocumento = ListaHistoricoGeracao;
                usuario.IdInstituicaoNavigation = instituicao;
                return usuario;
            }
            return null;
        }

        public async Task<UsuarioADE> ObterDetalhesUsuario(ClaimsPrincipal User)
        {
            UsuarioADE usuario;
            try
            {
                usuario = await _userManager.GetUserAsync(User);
            }
            catch (Exception)
            {
                usuario = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            if(usuario != null)
            {
                Curso curso = await _servicoCurso.BuscarPorId(usuario.IdCurso);
                usuario.IdCursoNavigation = curso;
            }
            return usuario;
        }

        public async Task<UsuarioADE> ObterUsuarioComDadosPessoais(UsuarioADE usuario)
        {
            _servicoRequisitoDeUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
            _servicoHistoricoGeracaoDocumento = new ServicoHistoricoGeracaoDocumento(ref unitOfWork);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            List<RequisitoDeUsuario> ListaRequisitoUsuario = await _servicoRequisitoDeUsuario.BuscarRequisitosDoUsuario(usuario.Id);
            List<HistoricoGeracaoDocumento> ListaHistoricoGeracao = await _servicoHistoricoGeracaoDocumento.RecuperarHistoricoDoUsuario(usuario.Id);
            Curso curso = await _servicoCurso.BuscarPorId(usuario.IdCurso);
            usuario.IdCursoNavigation = curso;
            usuario.HistoricoGeracaoDocumento = ListaHistoricoGeracao;
            return usuario;
        }

        public string ObterIdUsuario(ClaimsPrincipal User) => _userManager.GetUserId(User);

        public async Task<string> ObterEmailUsuario(UsuarioADE usuario) =>  await _userManager.GetEmailAsync(usuario);

        public async Task<string> ObterIdUsuario(UsuarioADE User) => await _userManager.GetUserIdAsync(User);

        public async Task<UsuarioADE> ObterUsuarioPorEmail(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<UsuarioADE> ObterUsuarioPorEmailOuId(string EmailOuId)
        {
            try
            {
                UsuarioADE usuario =  await _userManager.FindByEmailAsync(EmailOuId);
                if (usuario != null)
                {
                    return usuario;
                }
                else
                {
                    usuario = await _userManager.FindByIdAsync(EmailOuId);
                    if (usuario != null)
                    {
                        return usuario;
                    }
                    else
                    {
                        throw new Exception("Usuário não encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GerarCodigoDeConfirmacao(UsuarioADE usuario) => await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
        public async Task<string> GerarCodigoDeConfirmacao(string Email)
        {
            UsuarioADE usuario = await ObterUsuarioPorEmail(Email);
            return await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
        }

        public async Task<string> GerarTokenResetDeSenha(UsuarioADE usuario) => await _userManager.GeneratePasswordResetTokenAsync(usuario);

        public async Task<IdentityResult> ConfirmarEmail(UsuarioADE usuario, string code) => await _userManager.ConfirmEmailAsync(usuario, code);

        public async Task<bool> EmailConfirmado(UsuarioADE usuario) => await _userManager.IsEmailConfirmedAsync(usuario);

        public async Task<bool> ChecarSenhaDigitada(UsuarioADE usuario, string senha) => await _userManager.CheckPasswordAsync(usuario, senha);

        public async Task<IdentityResult> TrocarSenha(UsuarioADE usuario, string senhaAntiga, string senhaNova) => await _userManager.ChangePasswordAsync(usuario, senhaAntiga, senhaNova);

        public async Task<IdentityResult> ResetarSenha(UsuarioADE usuario, string Token, string senha) => await _userManager.ResetPasswordAsync(usuario, Token, senha);

        public async Task<IdentityResult> SetarTipoRecebimentoNotificacao(UsuarioADE usuario, EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao)
        {
            usuario.TipoRecebimentoNotificacao = TipoRecebimentoNotificacao;
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task RefreshSignIn(UsuarioADE usuario) {
            await _signInManager.RefreshSignInAsync(usuario);
        }

        public bool ValidarEmail(string email)
        {
            if (email != null)
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            return false;
        }
        
        public async Task<IdentityResult> AtualizarAsync(UsuarioADE usuario, UsuarioADE entity)
        {
            usuario.DataHoraUltimaAlteracao = DateTime.Now;
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> AtualizarEmail(UsuarioADE usuario, string email)
        {
            await AtualizarAsync(usuario, null);
            return await _userManager.SetEmailAsync(usuario, email);
        }

        public async Task<IdentityResult> RemoverAsync(UsuarioADE usuario, UsuarioADE entity)
        {
            usuario.DataHoraExclusao = DateTime.Now;
            string Ticks = DateTime.Now.Ticks.ToString();
            usuario.Email = $"Removido_{Ticks}";
            usuario.NormalizedEmail = $"Removido_{Ticks}";
            usuario.UserName = $"Removido_{Ticks}";
            usuario.NormalizedUserName = $"Removido_{Ticks}";
            usuario.Logado = false;
            usuario.EmailConfirmed = false;
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task<UsuarioADE> BuscarPorId(string id)
        {
            UsuarioADE usuario = await _userManager.FindByIdAsync(id);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
            usuario.IdCursoNavigation = await _servicoCurso.BuscarPorId(usuario.IdCurso);
            return usuario;
        }

        public async Task<List<UsuarioADE>> Filtrar(Expression<Func<UsuarioADE, bool>> expression)
        {
            List<UsuarioADE> lista = await unitOfWork.RepositorioUsuario.Filtrar(expression);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            foreach (UsuarioADE usuario in lista){
                usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);    
                usuario.IdCursoNavigation = await _servicoCurso.BuscarPorId(usuario.IdCurso);    
            }
            return lista.OrderByDescending(x => x.Pontuacao).ToList();
        }

        public async Task<List<UsuarioADE>> Listar()
        {
            List<UsuarioADE> lista = await unitOfWork.RepositorioUsuario.ListarUsuarios();
            return lista.OrderByDescending(x => x.Pontuacao).ToList();
        }

        public async Task<List<UsuarioADE>> TakeBetween(int start, int finish)
        {
            List<UsuarioADE> lista = await unitOfWork.RepositorioUsuario.TakeBetween(start,finish);
            _servicoCurso = new ServicoCurso(ref unitOfWork);
            _servicoInstituicao = new ServicoInstituicao(ref unitOfWork);
            foreach (UsuarioADE usuario in lista)
            {
                usuario.IdInstituicaoNavigation = await _servicoInstituicao.BuscarPorId(usuario.IdInstituicao);
                usuario.IdCursoNavigation = await _servicoCurso.BuscarPorId(usuario.IdCurso);
            }
            return lista.OrderByDescending(x => x.Pontuacao).ToList();
        }

        public async Task<int> CountByInstituicao(int idInstituicao) => await unitOfWork.RepositorioUsuario.CountByInstituicao(idInstituicao);
        
        public async Task<int> CountByCurso(int idCurso) => await unitOfWork.RepositorioUsuario.CountByCurso(idCurso);

        public async Task<int> Count() => await unitOfWork.RepositorioUsuario.Count();

        public async Task<int> CountLoggedIn() => await unitOfWork.RepositorioUsuario.CountLoggedIn();
    }
}
