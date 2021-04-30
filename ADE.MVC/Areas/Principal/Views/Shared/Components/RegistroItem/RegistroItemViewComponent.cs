using ADE.Dominio.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.RegistroItem
{
    public class RegistroItemViewComponent: ViewComponent
    {
        public RegistroItemViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(RegistroDeHoras registro)
        {
            return View(registro);
        }
    }
}
