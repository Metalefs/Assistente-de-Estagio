using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Dominio.Models
{
    public class RetornoUploadDocumento
    {
        public string TextoDocumento { get; set; }
        public string TextoDocumentoHTML { get; set; }
        public ComparacaoRequisitos ComparacaoRequisitos { get; set; }

        public RetornoUploadDocumento(string textoDocumento, string textoDocumentoHTML, ComparacaoRequisitos comparacaoRequisitos)
        {
            TextoDocumento = textoDocumento;
            ComparacaoRequisitos = comparacaoRequisitos;
            TextoDocumentoHTML = textoDocumentoHTML;
        }
    }
}
