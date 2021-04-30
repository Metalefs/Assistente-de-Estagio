using ADE.Dominio.Models;
using System;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class CartaoDocumentoViewmodel
    {
        public Documento Documento { get; set; }
        public bool Preenchido { get; set; }
        public DateTime DataPreenchimento { get; set; }

        public CartaoDocumentoViewmodel(Documento documento, bool preenchido, DateTime dataPreenchimento)
        {
            Documento = documento;
            Preenchido = preenchido;
            DataPreenchimento = dataPreenchimento;
        }

        public CartaoDocumentoViewmodel(Documento documento, bool preenchido)
        {
            Documento = documento;
            Preenchido = preenchido;
        }
    }
}
