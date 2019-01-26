using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assistente_de_Estagio.Models;

namespace Assistente_de_Estagio.Models
{
    public class DocumentoCursoRegistroAluno
    {
        public IList<Documento> Documentos { get; set; }
        public IList<Curso> Cursos { get; set; }
        public IList<Ficharegistro> Fichas { get; set; }
        public IList<Usuario> Usuarios { get; set; }
    }
}
