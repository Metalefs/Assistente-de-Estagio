using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.FormularioAdicionarAtividadeUsuarioViewComponent
{
    public class FormularioAdicionarAtividadeUsuarioViewComponent : ViewComponent
    {
        public FormularioAdicionarAtividadeUsuarioViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           return View();
        }
    }
}
