using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class AlterarDocumentoViewModel
    {
        [Required]
        public Documento Documento { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<RequisitoDeDocumento> Requisitos { get; set; }
        public string IdCurso { get; set; }
        public string TipoDocumento { get; set; }
        public string EtapaDocumento { get; set; }
        public IFormFile Arquivo { get; set; }
        public IFormFile PDFArquivo { get; set; }
        public int SelectedCurso { get; set; }

        public AlterarDocumentoViewModel()
        {
        }

        public AlterarDocumentoViewModel(Documento documento, int selection, List<Curso> cursos, List<RequisitoDeDocumento> requisitos)
        {
            Cursos = cursos;
            Documento = documento;
            SelectedCurso = selection;
            Requisitos = requisitos;
        }
    }
}
