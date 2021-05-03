using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.FormStepCircleViewComponent
{
    public class FormStepCircleViewComponent : ViewComponent
    {
        public FormStepCircleViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(int passos)
        { 
            return View(passos);
        }
    }
}
