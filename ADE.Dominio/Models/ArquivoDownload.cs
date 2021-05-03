using ADE.Dominio.Models.Enums;
using System.IO;

namespace ADE.Dominio.Models
{
    public class ArquivoDownload
    {
        public MemoryStream Bytes { get; set; }
        public string TipoArquivo { get; set; }

        public ArquivoDownload(MemoryStream _Bytes, EnumFormatoDocumento tipoArquivo)
        {
            this.Bytes = _Bytes;
            switch (tipoArquivo)
            {
                case EnumFormatoDocumento.docx:
                    TipoArquivo = "application/msword";
                    break;
                case EnumFormatoDocumento.pdf:
                    TipoArquivo = "application/pdf";
                    break;
            }
        }

    }
}
