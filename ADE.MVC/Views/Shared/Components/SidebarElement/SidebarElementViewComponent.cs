using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ADE.Apresentacao.Models;

namespace Assistente_de_Estagio.Views.Shared.Components.SidebarElement
{
    public class SideBarElementViewComponent : ViewComponent
    {
        IHostingEnvironment env;
        public SideBarElementViewComponent(IHostingEnvironment env)
        {
            this.env = env;
        }

        public async Task<IViewComponentResult> InvokeAsync(Elemento elemento, string tipo = "bootstrap")
        {
            string view = tipo;
            return View(view, elemento);
        }

    }
}
