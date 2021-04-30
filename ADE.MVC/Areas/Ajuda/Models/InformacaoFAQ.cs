using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Ajuda.Models
{
    public class InformacaoFAQ
    {
        public FAQ FAQ { get; set; }
        public UsuarioADE UsuarioPergunta { get; set; }
        public UsuarioADE UsuarioResposta { get; set; }

        public InformacaoFAQ(FAQ fAQ, UsuarioADE usuarioPergunta, UsuarioADE usuarioResposta)
        {
            FAQ = fAQ;
            UsuarioPergunta = usuarioPergunta;
            UsuarioResposta = usuarioResposta;
        }
    }
}
