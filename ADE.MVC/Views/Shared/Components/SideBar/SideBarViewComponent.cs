using ADE.Apresentacao.Models.Sidenav;
using ADE.Dominio.Models.Individuais;
using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.SideBarViewComponent
{
    public class SideBarViewComponent : ViewComponent
    {
        IHostingEnvironment env;
        public static Sidebar Sidebar { get; set; }
        public SideBarViewComponent(IHostingEnvironment env)
        {
            this.env = env;
            if(Sidebar == null)
                Sidebar = DesserializarSideBar();
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario, ScreenType screenType)
        {
            string view = screenType == ScreenType.Desktop ? "Default" : "Mobile";
            Sidebar.UsuarioADE = usuario;
            Sidebar.LoadType = usuario == null || usuario != null && usuario.Id == "N/A" ? LoadType.Skeleton : LoadType.Full;
            return View(view,Sidebar);
        }

        private Sidebar DesserializarSideBar()
        {
            string path = Path.Combine(env.WebRootPath, "Sidebar.json");
            string sidebarJson = File.ReadAllText(path, System.Text.Encoding.GetEncoding("iso-8859-1"));

            return JsonConvert.DeserializeObject<Sidebar>(sidebarJson);
        }
    }
}
