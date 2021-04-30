using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Ajuda.Models
{
    public class FAQViewModel
    {
        public PaginatedList<InformacaoFAQ> FAQS { get; set; }
        public Instituicao Instituicao { get; set; }
        public int QuantidadePerguntas { get; set; }
        public int QuantidadePerguntasRespondidas { get; set; }
        public int QuantidadePerguntasNaoRespondidas { get; set; }
    }
}
