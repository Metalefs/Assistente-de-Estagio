using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Apresentacao.Models;

namespace Assistente_de_Estagio.Views.Shared.Components.UserBodyViewComponent
{
    public class UserBodyViewComponent : ViewComponent
    {
        public UserBodyViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(UserInterface ui)
        {
            return View(ui);
        }
    }
}
