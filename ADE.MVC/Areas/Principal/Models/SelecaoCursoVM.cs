using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class SelecaoCursoVM
    {
        public List<string> NomeCurso { get; set; }
        public List<int> IdCurso { get; set; }

        public SelecaoCursoVM()
        {
            NomeCurso = new List<string>();
            IdCurso = new List<int>();
        }
    }
}
