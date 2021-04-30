using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Requisitodedocumento
    {
        public int? DocumentoIdDocumento { get; set; }
        public byte? OrdemRequisito { get; set; }
        public int RequisitosIdRequisito { get; set; }
        public int IdRequisitoDeDocumento { get; set; }
    }
}
