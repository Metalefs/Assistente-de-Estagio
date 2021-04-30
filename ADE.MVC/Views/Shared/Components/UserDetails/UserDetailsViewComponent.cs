using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Views.Shared.Components.UserDetails;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.UserDetailsViewComponent
{
    public class UserDetailsViewComponent : ViewComponent
    {
        
        public UserDetailsViewComponent() : base()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(UsuarioADE usuario, string IDComponente)
        {
            UserDetailsViewModel model = new UserDetailsViewModel(usuario, IDComponente); 
            return View(model);
        }
    }
}
