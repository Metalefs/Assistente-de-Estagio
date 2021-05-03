using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.TabelaCursosViewComponent
{
    public class TabelaCursosViewComponent : ViewComponent
    {
        public TabelaCursosViewComponent()
        {  }
        

        public async Task<IViewComponentResult> InvokeAsync(PaginatedList<InformacaoCursoVM> InfoCursos)
        {
            return View(InfoCursos);
        }
    }
}
