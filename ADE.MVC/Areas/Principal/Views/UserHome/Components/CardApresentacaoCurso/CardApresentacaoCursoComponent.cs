using ADE.Dominio.Models;
using Assistente_de_Estagio.Areas.Principal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.CardApresentacaoCursoViewComponent
{
    public class CardApresentacaoCursoViewComponent : ViewComponent
    {
        public CardApresentacaoCursoViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(Curso Curso, string texto = null)
        {
            CardApresentacaoViewmodel<Curso> model;
            if (texto == null)
                model = new CardApresentacaoViewmodel<Curso>(Curso);
            else
            {
                model = new CardApresentacaoViewmodel<Curso>(Curso, texto);
            }
            return View(model);
        }
    }
}
