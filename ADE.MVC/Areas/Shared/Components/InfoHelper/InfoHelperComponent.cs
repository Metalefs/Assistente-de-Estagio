using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Acesso.Components.InfoHelperComponentViewComponent
{
    public class InfoHelperViewComponent : ViewComponent
    {
       public InfoHelperViewComponent(
        )
       {
       }

        public async Task<IViewComponentResult> InvokeAsync(object urlInfo)
        {
            return View("Default",(string)urlInfo);
        }

    }
}
