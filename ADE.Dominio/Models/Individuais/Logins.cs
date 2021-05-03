using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models.Individuais
{
    public class Logins : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string IdUsuario { get; set; }
        public virtual DateTime DataHoraLogin { get; set; }
        public virtual DateTime DataHoraLogout { get; set; }

        public Logins(string idUsuario, DateTime dataHoraLogin)
        {
            IdUsuario = idUsuario;
            DataHoraLogin = dataHoraLogin;
        }

        public Logins() { }
        public virtual UsuarioADE Usuario { get; set; }
    }
}
