using ADE.Dominio.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class MensagemIndividual : ModeloBase
    {
        [Key]
        public override int Identificador { get; set; }
        public string IdUsuarioRemetente { get; set; }
        public string IdUsuarioDestino { get; set; }
        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
        public string Conteudo { get; set; }
        public EnumStatusMensagem Status { get; set; }

        public MensagemIndividual(string idUsuarioRemetente, string idUsuarioDestino, string conteudo)
        {
            IdUsuarioRemetente = idUsuarioRemetente;
            IdUsuarioDestino = idUsuarioDestino;
            Conteudo = conteudo;
            Status = EnumStatusMensagem.Enviado;
        }
    }
}
