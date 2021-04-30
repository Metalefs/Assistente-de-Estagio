using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Views.Shared.Components.UserDetails
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(UsuarioADE usuario, string iDComponente)
        {
            Usuario = usuario;
            IDComponente = iDComponente;
        }

        public UsuarioADE Usuario { get; set; }
        public string IDComponente { get; set; }
    }
}
