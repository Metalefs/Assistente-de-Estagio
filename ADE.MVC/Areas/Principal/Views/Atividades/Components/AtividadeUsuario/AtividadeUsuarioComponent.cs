using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.AtividadeUsuarioViewComponent
{
    public class AtividadeUsuarioViewComponent : ViewComponent
    {
        public AtividadeUsuarioViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(AtividadeUsuario atividade)
        {
           return View(atividade);
        }
    }
}
