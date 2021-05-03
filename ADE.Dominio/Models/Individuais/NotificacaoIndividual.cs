using ADE.Dominio.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class NotificacaoIndividual : ModeloBase
    {
        [Key]
        public override int Identificador { get; set; }
        public string IdUsuarioRemetente { get; set; }
        public string IdUsuarioDestino { get; set; }
        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
        public string Cabecalho { get; set; }
        public string Conteudo { get; set; }
        public EnumStatusNotificacaoIndividual Status { get; set; }

        public NotificacaoIndividual()
        {
        }

        public NotificacaoIndividual(string idUsuarioRemetente, string idUsuarioDestino, string cabecalho, string conteudo, EnumStatusNotificacaoIndividual status = EnumStatusNotificacaoIndividual.Enviado)
        {
            IdUsuarioRemetente = idUsuarioRemetente;
            IdUsuarioDestino = idUsuarioDestino;
            Cabecalho = cabecalho;
            Conteudo = conteudo;
            Status = status;
        }
    }
}
