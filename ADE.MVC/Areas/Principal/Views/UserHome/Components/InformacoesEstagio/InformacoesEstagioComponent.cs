using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.InformacoesEstagioViewComponent
{
    public class InformacoesEstagioViewComponent : ViewComponent
    {
        public InformacoesEstagioViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
