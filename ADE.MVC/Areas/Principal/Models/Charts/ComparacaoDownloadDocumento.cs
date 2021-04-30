using ADE.Dominio.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Principal.Models.Charts
{
    public class ComparacaoDownloadDocumento
    {
        private List<int> IdsTodosDocumentosDoCurso { get; set; }
        private List<int> IdsDocumentosBaixados { get; set; }
        public int ProgressoUsuario { get; set; }

        public ComparacaoDownloadDocumento(List<Documento> _IdsTodosDocumentosDoCurso, List<HistoricoGeracaoDocumento> _IdsDocumentosBaixados)
        {
            IdsTodosDocumentosDoCurso = new List<int>();
            IdsDocumentosBaixados = new List<int>();
            ObterIdsDocumento(_IdsTodosDocumentosDoCurso);
            ObterIdsHistoricoGeracao(_IdsDocumentosBaixados);

            if (IsValidForComparison())
            {
                CalcularProgressoDoUsuario();
            }
            else
            {
                ProgressoUsuario = 0;
            }
        }

        private void ObterIdsDocumento(List<Documento> documentos)
        {
            foreach(Documento documento in documentos)
            {
                IdsTodosDocumentosDoCurso.Add(documento.Identificador);
            }
        }

        private void ObterIdsHistoricoGeracao(List<HistoricoGeracaoDocumento> historicos)
        {
            foreach (HistoricoGeracaoDocumento historico in historicos)
            {
                IdsDocumentosBaixados.Add(historico.Documento);
            }
        }

        private bool IsValidForComparison()
        {
            return (IdsTodosDocumentosDoCurso.Count > 0 && IdsDocumentosBaixados.Count > 0);
        }

        private void CalcularProgressoDoUsuario()
        {
            List<int> Diferenca = IdsTodosDocumentosDoCurso.Except(IdsDocumentosBaixados).ToList();
            ProgressoUsuario = IdsTodosDocumentosDoCurso.Count() - Diferenca.Count();
        }

    }
}
