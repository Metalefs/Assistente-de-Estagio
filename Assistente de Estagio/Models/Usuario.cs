using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; }
        public DateTime? DataCriacaoUltimoDocumento { get; set; }
        public int DocumentoIdDocumento { get; set; }

        public virtual Documento DocumentoIdDocumentoNavigation { get; set; }
    }
}
