using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Requisitodeusuario
    {
        public int? UsuarioIdUsuario { get; set; }
        public int? RequisitosIdRequisito { get; set; }
        public string Dados { get; set; }
        public int IdRequisistoDeUsuario { get; set; }
    }
}
