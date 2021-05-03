using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.ExibicaoInstituicaoViewComponent
{
    public class ExibicaoInstituicaoViewComponent : ViewComponent
    {
        public ExibicaoInstituicaoViewComponent()
        {  }
        

        public async Task<IViewComponentResult> InvokeAsync(Instituicao instituicao)
        {
            return View(instituicao);
        }
    }
}
