using ADE.Apresentacao.Models.NavTab;
using ADE.Dominio.Models.Individuais;
using ADE.Utilidades.Seguranca;
using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ADE.Utilidades.Extensions;

namespace Assistente_de_Estagio.Views.Shared.Components.NavTabViewComponent
{
    public class NavTabViewComponent : ViewComponent
    {
        public static NavTab NavTab {get; set;}
        public static NavTab NavTabAdmin {get; set;}
        private IConfiguration Configuration;
        private IHostingEnvironment env;
        public NavTabViewComponent(IHostingEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.Configuration = configuration;
            if (NavTab == null)
                NavTab = DesserializarNavTab();
            if(NavTabAdmin == null)
                NavTabAdmin = DesserializarNavTab("admin-nav-tab.json", false);
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario, AreaComponente Area = AreaComponente.Usuario)
        {
            string view = Area.GetDescription();
            NavTab model = Area == AreaComponente.Admin ? NavTabAdmin : NavTab;
            model.LoadType = usuario == null || usuario != null && usuario.Id == "N/A" ? LoadType.Skeleton : LoadType.Full;
            return View(view, model);
        }

        private NavTab DesserializarNavTab(string navtab_path = "nav-tab.json", bool encrypted = false)
        {
            string path = Path.Combine(env.WebRootPath, navtab_path);
            string navTabJson = File.ReadAllText(path, System.Text.Encoding.GetEncoding("iso-8859-1"));
            return encrypted == true ? JsonConvert.DeserializeObject<NavTab>(Criptografia.Decriptografar(Convert.FromBase64String(navTabJson), Encoding.UTF8.GetBytes(Configuration.GetSection("Schlussel").Value))) : JsonConvert.DeserializeObject<NavTab>(navTabJson);
        }
    }
}
