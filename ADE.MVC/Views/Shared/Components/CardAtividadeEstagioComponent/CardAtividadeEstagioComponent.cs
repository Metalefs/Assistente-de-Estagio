using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Shared.Components.CardAtividadeEstagioComponent
{
    public class CardAtividadeEstagioComponent : ViewComponent
    {
        public CardAtividadeEstagioComponent(
        )
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(AtividadeEstagio atividade)
        {
            return View(atividade);
        }

    }
}
