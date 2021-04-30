using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Assistente_de_Estagio.Areas.Principal.Models;
using Assistente_de_Estagio.Areas.Shared;
using ADE.Dominio.Models;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.TabelaDocumentosViewComponent
{
    public class TabelaDocumentosViewComponent : ViewComponent
    {
        public TabelaDocumentosViewComponent()
        {  }
        

        public async Task<IViewComponentResult> InvokeAsync(PaginatedList<Documento> documentos, List<HistoricoGeracaoDocumento> historico)
        {
            TabelaDocumentosVM model = new TabelaDocumentosVM(documentos,historico);
            return View(model);
        }
    }
}
