using ADE.Dominio.Models.Individuais;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models.RelacaoEntidades
{
    public class VisualizacaoNotificacaoGeral : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string IdUsuario { get; set; }
        public int IdNotificacao { get; set; }
       
        public VisualizacaoNotificacaoGeral() { }

        public VisualizacaoNotificacaoGeral(string idUsuario, int idNotificacao)
        {
            IdUsuario = idUsuario;
            IdNotificacao = idNotificacao;
        }

        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
        public virtual AlteracaoEntidadesSistema IdNotificacaoNavigation { get; set; }
    }
}
