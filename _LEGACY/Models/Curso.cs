using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Curso
    {
        public int IdCurso { get; set; }
        public string NomeCurso { get; set; }
        public string DescricaoCurso { get; set; }
        public string CoordenadorCurso { get; set; }
        public string FaculdadeCurso { get; set; }
        public int DocumentoIdDocumento { get; set; }

        public virtual Documento DocumentoIdDocumentoNavigation { get; set; }
    }
}
