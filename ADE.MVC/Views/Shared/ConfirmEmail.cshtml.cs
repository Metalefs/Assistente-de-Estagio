using Microsoft.AspNetCore.Authorization;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assistente_de_Estagio.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<UsuarioADE> _userManager;

        public ConfirmEmailModel(UserManager<UsuarioADE> userManager)
        {
            _userManager = userManager;
        }

        //public async Task<IActionResult> OnGetAsync(string user, string code)
        //{
        //    if (user == null || code == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { area = "Acesso" });
        //    }

        //    UsuarioADE usuario = await _userManager.FindByEmailAsync(user);
        //    IdentityResult result = await _userManager.ConfirmEmailAsync(usuario, code);
        //    if (result.Succeeded)
        //    {
        //        return Page();
        //    }
        //    else
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //        if (!result.Succeeded)
        //        {
        //            throw new InvalidOperationException($"Erro ao confirmar o email para o usuario: '{user}':");
        //        }
        //        return RedirectToAction("Index", "ListagemDocumentos", new { area = "Principal", idCurso = usuario.IdCurso, sortOrder = string.Empty, currentFilter = string.Empty, searchString = string.Empty, p = 0, idUsuario = usuario.Id });
        //    }
        //}
    }
}
