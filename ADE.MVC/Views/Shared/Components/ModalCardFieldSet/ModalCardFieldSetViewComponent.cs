using Assistente_de_Estagio.Areas.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.ModalCardFieldSetViewComponent
{
    public class ModalCardFieldSetViewComponent : ViewComponent
    {
        public ModalCardFieldSetViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(ModalCardViewModel Modal, bool CanClose = true, bool DefaultOpen = true)
        {
            Modal.CanClose = CanClose;
            Modal.DefaultOpen = DefaultOpen;
            return View(Modal);
        }
    }
}
