using ADE.Dominio.Models.Individuais;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models
{
    public class HistoricoGeracaoDocumento : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int Documento { get; set; }
        public virtual string IdUsuario { get; set; }

        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
        public virtual Documento IdDocumentoNavigation { get; set; }

        public HistoricoGeracaoDocumento(string descricao, int documento, string idUsuario)
        {
            Descricao = descricao;
            Documento = documento;
            IdUsuario = idUsuario;
        }

        public override string ToString()
        {
            return Descricao;
        }
    }
}
