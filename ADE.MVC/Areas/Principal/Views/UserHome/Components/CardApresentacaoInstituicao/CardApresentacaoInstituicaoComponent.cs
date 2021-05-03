using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using System;
using Assistente_de_Estagio.Areas.Principal.Models;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.CardApresentacaoInstituicaoViewComponent
{
    public class CardApresentacaoInstituicaoViewComponent : ViewComponent
    {
        public CardApresentacaoInstituicaoViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(Instituicao Instituicao, string texto = null)
        {
            CardApresentacaoViewmodel<Instituicao> model;
            if (texto == null)
                model = new CardApresentacaoViewmodel<Instituicao>(Instituicao);
            else
            {
                model = new CardApresentacaoViewmodel<Instituicao>(Instituicao, texto);
            }
            return View(model);
        }
    }
}
