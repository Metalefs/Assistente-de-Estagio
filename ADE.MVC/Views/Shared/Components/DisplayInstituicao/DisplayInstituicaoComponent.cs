using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.DisplayInstituicaoViewComponent
{
    public class DisplayInstituicaoViewComponent : ViewComponent
    {
        public DisplayInstituicaoViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(Instituicao instituicao)
        { 
            return View(instituicao);
        }
    }
}
