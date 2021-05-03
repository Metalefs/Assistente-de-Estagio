using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class OpcaoRequisito : ModeloBase
    {
        [Key]
        [Column(Order = 0)]
        public int IdRequisito { get; set; }
        [Key]
        [Column(Order = 1)]
        public string Valor { get; set; }
        public override int Identificador { get; set; }
        
        public OpcaoRequisito(int IdRequisito, string valor)
        {
            this.IdRequisito = IdRequisito;
            this.Valor = valor;
        }

        public OpcaoRequisito(string valor)
        {
            this.Valor = valor;
        }
        [JsonIgnore]
        public virtual Requisito IdRequisitoNavigation { get; set; }
    }
}
