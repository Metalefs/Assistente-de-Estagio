using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class GerenciamentoCursoViewmodel
    {
        public PaginatedList<Curso> Cursos { get; set; }
        public List<Instituicao> Instituicoes { get; set; }

        public GerenciamentoCursoViewmodel(PaginatedList<Curso> cursos, List<Instituicao> instituicoes)
        {
            Cursos = cursos;
            Instituicoes = instituicoes;
        }
    }
}
