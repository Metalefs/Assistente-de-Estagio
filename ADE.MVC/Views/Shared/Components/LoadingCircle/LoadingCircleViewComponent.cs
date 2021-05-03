using Assistente_de_Estagio.Views.Shared.Components.LoadingCircleViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Assistente_de_Estagio.Views.Shared.Components.LoadingCircleViewComponent
{
    public class LoadingCircleViewComponent : ViewComponent
    {
        public LoadingCircleViewComponent()
        { }

        public async Task<IViewComponentResult> InvokeAsync(string id, bool Closed = true)
        {
            LoadingCircle model = new LoadingCircle(id,Closed);
            return View(model);
        }
    }
}
