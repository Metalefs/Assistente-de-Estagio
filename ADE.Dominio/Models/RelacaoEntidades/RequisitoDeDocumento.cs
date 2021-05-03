using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class RequisitoDeDocumento : ModeloBase
    {
        public RequisitoDeDocumento(int idDocumento, int idRequisito)
        {
            IdDocumento = idDocumento;
            IdRequisito = idRequisito;
        }

        public RequisitoDeDocumento()
        {
        }

        [Key]
        [Column(Order = 0)]
        public virtual int IdDocumento { get; set; }
        [Key]
        [Column(Order = 1)]
        public virtual int IdRequisito { get; set; }
        public sbyte OrdemRequisito { get; set; }
        public override int Identificador { get; set; }
        [JsonIgnore]
        public virtual Documento IdDocumentoNavigation { get; set; }
        [JsonIgnore]
        public virtual Requisito IdRequisitoNavigation { get; set; }
    }
}
