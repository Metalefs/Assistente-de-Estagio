using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Data;
using ADE.Infra.Data.UOW;
using Microsoft.AspNetCore.Identity;
using ADE.Aplicacao.Services;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Shared.Chat.Components.ChatViewComponent
{
    public class ChatViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoListaAmigos ServicoListaAmigos;
        ServicoUsuario ServicoUsuario;

        public ChatViewComponent(ApplicationDbContext context, UserManager<UsuarioADE> userManager)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoUsuario = new ServicoUsuario(unitOfWork, userManager);
            ServicoListaAmigos = new ServicoListaAmigos(ref unitOfWork, userManager);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UsuarioADE usuario = await ServicoUsuario.ObterUsuarioLogado(UserClaimsPrincipal);
            List<UsuarioADE> amigos = new List<UsuarioADE>();
            if (usuario != null)
            {
                amigos = await ServicoListaAmigos.BuscarAmigosUsuario(usuario.Id);
            }
            ChatVM model = new ChatVM(amigos, usuario);
            return View(model);
        }

    }

    public class ChatVM
    {
        public List<UsuarioADE> Amigos { get; set; }
        public UsuarioADE Usuario { get; set; }

        public ChatVM(List<UsuarioADE> amigos, UsuarioADE usuario)
        {
            Amigos = amigos;
            Usuario = usuario;
        }
    }
}
