using Assistente_de_Estagio.Areas.Shared;
using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.ModalCardViewComponent
{
    public class ModalCardViewComponent : ViewComponent
    {
        public ModalCardViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(ModalCardViewModel Modal, bool CanClose = true, bool DefaultOpen = true)
        {
            Modal.CanClose = CanClose;
            Modal.DefaultOpen = DefaultOpen;
            string view = Modal.Tipo == TipoModal.Mini ? "Mini" : "Default";
            return View(view,Modal);
        }
    }
}
