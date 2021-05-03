using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Shared.Chat.Components.ChatCampoMensagemViewComponent
{
    public class ChatCampoMensagemViewComponent : ViewComponent
    {
        public ChatCampoMensagemViewComponent(
        )
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
