using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Html;

namespace ADE.Apresentacao.Models
{
    public class UserInterface
    {
        public IHtmlContent Body {get; set;}
        public UsuarioADE Usuario { get; set; }

        public UserInterface(IHtmlContent body, UsuarioADE usuario)
        {
            this.Body = body;
            this.Usuario = usuario;
        }
    }
}
