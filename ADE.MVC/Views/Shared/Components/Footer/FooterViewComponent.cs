using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.FooterViewComponent
{
    public class FooterViewComponent : ViewComponent
    {
        public FooterViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(string Versao)
        { 
            return View("Default",Versao);
        }
    }
}
