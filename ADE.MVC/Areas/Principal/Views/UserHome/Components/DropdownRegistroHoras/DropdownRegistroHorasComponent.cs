using ADE.Dominio.Models;
using Assistente_de_Estagio.Areas.Principal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.DropdownRegistroHorasComponentViewComponent
{
    public class DropdownRegistroHorasViewComponent : ViewComponent
    {
        public DropdownRegistroHorasViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
