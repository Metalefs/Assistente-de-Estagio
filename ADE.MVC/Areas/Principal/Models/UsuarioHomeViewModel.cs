using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class UsuarioHomeViewModel
    {
        public UsuarioADE Usuario { get; set; }
        public List<HistoricoGeracaoDocumento> GeracoesDocumento { get; set; }
        public List<InformacaoCursoVM> Cursos { get; set; }
        public int ContagemRequisitoDoUsuario { get; set; }
        public bool PrimeiroCurso { get; set; }
        public int QuantidadeDocumentosGerados;
        public int ProgressoDeEstagio;
        public int ContagemDocumentosCurso;

        public UsuarioHomeViewModel(UsuarioADE usuario, List<HistoricoGeracaoDocumento> geracoesDocumento, int requisitoDoUsuario, int quantidadeDocumentosGerados, int progressoDeEstagio)
        {
            Usuario = usuario;
            GeracoesDocumento = geracoesDocumento;
            ContagemRequisitoDoUsuario = requisitoDoUsuario;
            QuantidadeDocumentosGerados = quantidadeDocumentosGerados;
            ProgressoDeEstagio = progressoDeEstagio;
        }
        public UsuarioHomeViewModel()
        {}
    }
}
