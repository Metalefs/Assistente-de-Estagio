using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        Task<IdentityResult> CadastrarAsync(UsuarioADE usuario, string password);
        Task<IList<string>> ObterAutorizacaoUsuario(UsuarioADE usuario);
        Task<IList<UsuarioADE>> ObterUsuariosPorFuncao(string roleName);
        Task<IdentityResult> CadastrarAdministradorAsync(UsuarioADE usuario, string password, bool existente, Models.Enums.EnumTipoUsuario Funcao);
        Task<ClaimsPrincipal> LogarUsuario(UsuarioADE user, string NameClaim);
        Task<SignInResult> LogarUsuario(UsuarioADE user, string Password, bool RememberMe, bool log);
        Task Logout(UsuarioADE usuario);
        Task<IdentityResult> AtualizarAsync(UsuarioADE usuario, UsuarioADE entity);
        Task<IdentityResult> RemoverAsync(UsuarioADE usuario, UsuarioADE entity);
        Task<UsuarioADE> ObterUsuarioLogado(ClaimsPrincipal User);
        string ObterIdUsuario(ClaimsPrincipal User);
        Task<UsuarioADE> ObterUsuarioComDadosPessoais(ClaimsPrincipal User);
        Task<UsuarioADE> ObterUsuarioComDadosPessoais(UsuarioADE User);
        Task<UsuarioADE> ObterDetalhesUsuario(ClaimsPrincipal User);
        Task<string> ObterIdUsuario(UsuarioADE User);
        Task<string> ObterEmailUsuario(UsuarioADE User);
        Task<UsuarioADE> ObterUsuarioPorEmail(string email);
        Task<UsuarioADE> ObterUsuarioPorEmailOuId(string EmailOuId);
        Task<string> GerarCodigoDeConfirmacao(UsuarioADE usuario);
        Task<string> GerarCodigoDeConfirmacao(string Email);
        Task<string> GerarTokenResetDeSenha(UsuarioADE usuario);
        Task<IdentityResult> ConfirmarEmail(UsuarioADE usuario, string code);
        Task<bool> EmailConfirmado(UsuarioADE usuario);
        Task<bool> ChecarSenhaDigitada(UsuarioADE usuario, string senha);
        Task<IdentityResult> TrocarSenha(UsuarioADE usuario, string senhaAntiga, string senhaNova);
        Task<IdentityResult> ResetarSenha(UsuarioADE usuario, string Token, string senha);
        Task<IdentityResult> SetarTipoRecebimentoNotificacao(UsuarioADE usuario, EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao);
        Task RefreshSignIn(UsuarioADE usuario);
        bool ValidarEmail(string email);       
        Task<List<UsuarioADE>> Listar();
        Task<List<UsuarioADE>> Filtrar(Expression<Func<UsuarioADE, bool>> expression);
        Task<UsuarioADE> BuscarPorId(string id);
        Task<int> Count();
        Task<int> CountByInstituicao(int IdInstituicao);
        Task<int> CountByCurso(int IdCurso);
        Task<int> CountLoggedIn();
    }
}
