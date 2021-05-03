using ADE.Dominio.Models;
using Assistente_de_Estagio.Areas.Principal.Models;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.TabelaDocumentosViewComponent
{
    public class TabelaDocumentosVM
    {
        public List<Documento> Documentos { get; set; }
        public List<HistoricoGeracaoDocumento> HistoricoGeracao { get; set; }

        public TabelaDocumentosVM(List<Documento> cartaoDocumento, List<HistoricoGeracaoDocumento> historicoGeracao)
        {
            Documentos = cartaoDocumento;
            HistoricoGeracao = historicoGeracao;
        }
    }
}
