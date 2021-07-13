using System.Collections.Generic;
using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ADE.Apresentacao.Areas.Principal.Controllers
{
    [AllowAnonymous]
    [Area("Ajuda")]
    public class SobreController : Controller
    {
        private IConfiguration Configuration;
        public SobreController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            var Time = Configuration.GetSection("Desenvolvedores:Time");
            List<Membro> Equipe = new List<Membro>();
            foreach (IConfigurationSection section in Time.GetChildren())
            {
                Equipe.Add(new Membro (section.GetValue<string>("Nome"), section.GetValue<string>("ImgSrc"), section.GetValue<string>("Funcao"), section.GetValue<string>("LinkedIn")));
            }
            return View(Equipe);
        }
    }
}