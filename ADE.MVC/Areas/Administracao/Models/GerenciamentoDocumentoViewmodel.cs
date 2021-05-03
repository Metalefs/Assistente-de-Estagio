using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class GerenciamentoDocumentoViewmodel
    {
        public PaginatedList<Documento> Documentos { get; set; }
        public List<Curso> Cursos { get; set; }
        public List<Instituicao> Instituicoes { get; set; }

        public GerenciamentoDocumentoViewmodel(PaginatedList<Documento> documentos, List<Curso> cursos, List<Instituicao> instituicoes)
        {
            Documentos = documentos;
            Cursos = cursos;
            Instituicoes = instituicoes;
        }
    }
}
