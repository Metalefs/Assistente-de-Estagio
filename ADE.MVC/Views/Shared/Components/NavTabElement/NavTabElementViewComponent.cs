using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.NavTabElementViewComponent
{
    public class NavTabElementViewComponent : ViewComponent
    {
        public NavTabElementViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(NavItem elemento, string tipo = "Default")
        {
            string view = tipo;
            return View(view, elemento);
        }

    }
}
