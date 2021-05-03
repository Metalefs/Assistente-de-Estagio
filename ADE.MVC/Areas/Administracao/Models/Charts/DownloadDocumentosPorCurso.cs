using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using ADE.Dominio.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public class DownloadDocumentosPorCurso : BaseChart, IChart
    {
        private List<HistoricoGeracaoDocumento> historico;
        public DownloadDocumentosPorCurso(List<HistoricoGeracaoDocumento> _historico)
        {
            Labels = new List<string>();
            Values = new List<int>();
            historico = _historico;
            Parse();
        }

        public override void Parse()
        {
            var historicoAgrupado = historico.GroupBy(
                curso => curso.IdDocumentoNavigation.IdCursoNavigation.Nome(),
                idDoc => idDoc.Documento,
                (curso, id) => new
                {
                    Key = curso,
                    Count = id.Count(),
                });

            foreach (var result in historicoAgrupado)
            {
                Labels.Add(result.Key);
                Values.Add(result.Count);
            }
        }
    }
}
