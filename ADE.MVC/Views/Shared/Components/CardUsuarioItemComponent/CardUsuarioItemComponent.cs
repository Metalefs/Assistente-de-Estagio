using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Shared.Chat.Components.CardUsuarioItemComponent
{
    public class CardUsuarioItemComponent : ViewComponent
    {
        public CardUsuarioItemComponent(
        )
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario, bool amigo = false, string view = "Default")
        {
            CardUsuarioVM model = new CardUsuarioVM(usuario,amigo);
            return View(view,model);
        }

    }

    public class CardUsuarioVM
    {
        public UsuarioADE Usuario {get;set;}
        public bool Amigo { get; set; }

        public CardUsuarioVM(UsuarioADE usuario, bool amigo)
        {
            this.Usuario = usuario;
            this.Amigo = amigo;
        }
    }
}
