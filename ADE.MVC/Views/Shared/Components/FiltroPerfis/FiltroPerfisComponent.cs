using ADE.Dominio.Models.RelacaoEntidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.FiltroPerfisViewComponent
{
    public class FiltroPerfisViewComponent : ViewComponent
    {
        public FiltroPerfisViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(List<ListaAmigos> amigos)
        {
           return View(amigos);
        }
    }
}
