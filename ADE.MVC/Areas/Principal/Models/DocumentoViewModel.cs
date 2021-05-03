using ADE.Dominio.Models;
using Assistente_de_Estagio.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Principal.Models
{
    public class DocumentoViewModel
    {
        public Documento Documento { get; set; }
        public List<Requisito> Requisitos { get; set; }
        public List<InformacaoDocumento> InformacaoDocumento { get; set; }
        public List<AreaEstagioCurso> AreasEstagioCurso { get; set; }

        public DocumentoViewModel(Documento documento, List<Requisito> requisitos, List<InformacaoDocumento> informacaoDocumento)
        {
            Documento = documento;
            Requisitos = requisitos;
            InformacaoDocumento = informacaoDocumento;
        }

        public DocumentoViewModel()
        {
        }
    }
}
