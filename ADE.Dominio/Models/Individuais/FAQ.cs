using ADE.Dominio.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class FAQ : ModeloBase
    {
        [Key]
        public override int Identificador { get ; set ; }
        [Required]
        public int IdInstituicao { get; set; }
        public virtual Instituicao Instituicao { get; set; }
        [Required]
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        [Required]
        public string IdUsuarioPergunta { get; set; }
        public string IdUsuarioResposta { get; set; }
        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
        public int Pontuacao { get; set; }
        [Required]
        public EnumStatusFAQ Status { get; set; }

        public FAQ(int idInstituicao, string pergunta, string resposta, string idUsuarioPergunta, string idUsuarioResposta = "", EnumStatusFAQ status = EnumStatusFAQ.Aberto)
        {
            IdInstituicao = idInstituicao;
            Pergunta = pergunta;
            Resposta = resposta;
            IdUsuarioPergunta = idUsuarioPergunta;
            IdUsuarioResposta = idUsuarioResposta;
            Pontuacao = 0;
            Status = status;
        }
        public FAQ()
        {
        }
    }
}
