using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Shared.Chat.Components.ChatSidebarViewComponent
{
    public class ChatSidebarViewComponent : ViewComponent
    {
        public ChatSidebarViewComponent(
        )
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(List<UsuarioADE> amigos)
        {
            return View(amigos);
        }

    }
}
