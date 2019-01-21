using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Jsonrequisitospreenchidos
    {
        public int IdJsonRequisitosPreenchidos { get; set; }
        public string DadosJson { get; set; }
        public int? UsuarioIdUsuario { get; set; }
        public int? DocumentoIdDocumento { get; set; }

        public virtual Documento DocumentoIdDocumentoNavigation { get; set; }
        public virtual Usuario UsuarioIdUsuarioNavigation { get; set; }
    }
}
