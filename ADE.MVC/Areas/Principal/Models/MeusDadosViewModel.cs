using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class MeusDadosViewModel
    {
        public UsuarioADE Usuario { get; set; }
        public PaginatedList<RequisitoDeUsuario> RequisitoDoUsuario { get; set; }
        public MeusDadosViewModel(UsuarioADE usuario, PaginatedList<RequisitoDeUsuario> requisitoDoUsuario)
        {
            Usuario = usuario;
            RequisitoDoUsuario = requisitoDoUsuario;
        }
        public MeusDadosViewModel()
        {}
    }
}
