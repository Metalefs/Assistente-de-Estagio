using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.AdminBodyViewComponent
{
    public class AdminBodyViewComponent : ViewComponent
    {
        public AdminBodyViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(UserInterface ui)
        {
            return View(ui);
        }
    }
}
