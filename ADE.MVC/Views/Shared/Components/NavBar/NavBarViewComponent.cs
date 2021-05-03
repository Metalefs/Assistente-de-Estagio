using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.NavBarViewComponent
{
    public class NavBarViewComponent : ViewComponent
    {
        public NavBarViewComponent() 
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario)
        {
            return View(usuario);
        }
    }
}
