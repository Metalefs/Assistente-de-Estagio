using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication;

namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Continuar conectado.")]
        public bool RememberMe { get; set; }
        public LoginViewModel() { }
        public LoginViewModel(string email, string password, bool rememberMe, IList<AuthenticationScheme> externalLogins)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
            ExternalLogins = externalLogins;
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
