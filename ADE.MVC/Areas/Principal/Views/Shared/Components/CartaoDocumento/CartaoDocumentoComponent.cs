using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models;
using System;
using Assistente_de_Estagio.Areas.Principal.Models;
namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.CartaoDocumentoViewComponent
{
    public class CartaoDocumentoViewComponent : ViewComponent
    {
        public CartaoDocumentoViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(Documento Documento, bool Preenchido, DateTime PreenchidotoEm)
        {
            CartaoDocumentoViewmodel model = new CartaoDocumentoViewmodel(Documento, Preenchido, PreenchidotoEm);
            return View(model);
        }
    }
}
