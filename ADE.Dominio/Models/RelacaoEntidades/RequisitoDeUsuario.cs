using ADE.Dominio.Models.Individuais;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class RequisitoDeUsuario : ModeloBase
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int IdRequisito { get; set; }
        public string Valor { get; set; }
        public override int Identificador { get; set; }
        [JsonIgnore]
        public virtual Requisito IdRequisitoNavigation { get; set; }
        public virtual UsuarioADE User { get; set; }

        public RequisitoDeUsuario(string userId, int idRequisito, string valor, UsuarioADE user)
        {
            UserId = userId;
            IdRequisito = idRequisito;
            Valor = valor;
            User = user;
        }
        public RequisitoDeUsuario() {}

        public override string ToString()
        {
            return $"{IdRequisito}: {Valor}";
        }
    }
}
