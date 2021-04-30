using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Shared
{
    public class FormularioPosLoginViewModel
    {
        public List<Instituicao> Instituicoes { get; set; }
        public int IdInstituicao { get; set; }
        public List<Curso> Cursos { get; set; }
        public int IdCurso { get; set; }
        public List<Requisito> Requisitos { get; set; }
    }
}
