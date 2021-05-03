using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADE.Dominio.Models.Individuais;
using ADE.Aplicacao.Services;

namespace Assistente_de_Estagio.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<UsuarioADE> _userManager;
        private readonly AuthMessageSender _emailSender;

        public ForgotPasswordModel(UserManager<UsuarioADE> userManager, AuthMessageSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        
    }
}
