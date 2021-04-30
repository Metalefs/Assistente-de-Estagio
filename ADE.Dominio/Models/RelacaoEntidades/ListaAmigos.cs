using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models.RelacaoEntidades
{
    public class ListaAmigos : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string IdUsuario { get; set; }
        public string IdAmigo { get; set; }
        public EnumTipoRelacionamento TipoRelacao {get; set;}

        public ListaAmigos() { }

        public ListaAmigos(string idUsuario, string idAmigo, EnumTipoRelacionamento tipoRelacao)
        {
            IdUsuario = idUsuario;
            IdAmigo = idAmigo;
            TipoRelacao = tipoRelacao;
        }

        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
    }
}
