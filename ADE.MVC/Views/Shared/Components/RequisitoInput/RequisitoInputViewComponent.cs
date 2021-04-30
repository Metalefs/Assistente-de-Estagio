using ADE.Dominio.Models;
using Assistente_de_Estagio.Views.Shared.Components.RequisitoInput;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.RequisitoInputViewComponent
{
    public class RequisitoInputViewComponent : ViewComponent
    {
        public RequisitoInputViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(Requisito requisito, string onchange)
        {
            RequisitoInputViewModel model = new RequisitoInputViewModel(requisito,onchange);
            return View(model);
        }
    }
}
