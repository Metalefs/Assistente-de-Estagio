using ADE.Dominio.Models.Enums;
using System.Security.Claims;

namespace ADE.Utilidades.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static bool ShowDefaultInterface(this ClaimsPrincipal User, string ViewdataTitle)
        {
            if (!User.Identity.IsAuthenticated && ViewdataTitle.Contains("Entrar"))
                return false;
            else if (User.Identity.IsAuthenticated && ViewdataTitle.Contains("Selecionar Instituicao"))
                return false;
            //else if (ViewdataTitle.Contains("Termos de Uso"))
            //    return true;
            //else if (!User.Identity.IsAuthenticated)
            //    return false;
            else
                return true;
        }
        public static bool IsAdminOrCriadorConteudo(this ClaimsPrincipal User)
        {
            return User.IsInRole(EnumTipoUsuario.Admin.GetDescription()) || User.IsInRole(EnumTipoUsuario.CriadorConteudo.GetDescription());
        }

        public static bool IsAdmin(this ClaimsPrincipal User)
        {
            return User.IsInRole(EnumTipoUsuario.Admin.GetDescription());
        }
        public static bool IsCriadorConteudo(this ClaimsPrincipal User)
        {
            return User.IsInRole(EnumTipoUsuario.CriadorConteudo.GetDescription());
        }
    }
}
