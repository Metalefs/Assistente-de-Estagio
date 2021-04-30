using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Testes.Constants
{
    public static class Nomes
    {
        public const string DocXMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public const string DocXFormat = "docx";
        public const string PDFFormat = "pdf";
        public static string ArquivoDocXValido => "Valido.docx";
        public static string ArquivoDocXInvalido = "Invalido.mp3";
    }

    public static class Tamanhos
    {
        public const int TamanhoMaximoDocX = 2097152;
    }
}
