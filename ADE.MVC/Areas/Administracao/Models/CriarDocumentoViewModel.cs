using ADE.Dominio.Models;
using Microsoft.AspNetCore.Http;
using ADE.Dominio.Models.Individuais;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ADE.Dominio.Models.Enums;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class CriarDocumentoViewModel
    {
        [Required]
        public Documento Documento { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<Instituicao> Instituicoes { get; set; }
        [Required]
        public int IdInstituicao { get; set; }
        [Required]
        public int IdCurso { get; set; }
        [Required]
        public IFormFile Arquivo { get; set; }
        public IFormFile ArquivoPDF { get; set; }

        public CriarDocumentoViewModel()
        {
        }
        public CriarDocumentoViewModel(Documento documento, int idCurso, List<Curso> cursos, int idInstituicao, List<Instituicao> instituicoes)
        {
            Instituicoes = instituicoes;
            Cursos = cursos;
            Documento = documento;
            IdInstituicao = idInstituicao;
            IdCurso = idCurso;
        }
    }
}
