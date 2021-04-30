using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Assistente_de_Estagio.Areas.Principal.Models;
namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.ExibicaoCursoViewComponent
{
    public class ExibicaoCursoViewComponent : ViewComponent
    {
        public ExibicaoCursoViewComponent()
        {  }
        

        public async Task<IViewComponentResult> InvokeAsync(InformacaoCursoVM curso)
        {
            return View(curso);
        }
    }
}
