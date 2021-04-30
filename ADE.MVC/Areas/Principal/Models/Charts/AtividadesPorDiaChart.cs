using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using System.Collections.Generic;
using System.Linq;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public class AtividadesPorDiaChart : BaseChart, IChart
    {
        private List<LogAcoesEspeciais> atividades;
        public AtividadesPorDiaChart(List<LogAcoesEspeciais> _logins)
        {
            Labels = new List<string>();
            Values = new List<int>();
            atividades = _logins;
            Parse();
        }

        public override void Parse()
        {
            var atividadesAgrupadas = atividades.GroupBy(
               dataAtividade => dataAtividade.DataHoraInclusao.Date,
               id => id.Identificador,
               (dataAtividade, id) => new
               {
                   Key = dataAtividade,
                   Count = id.Count()
               });

            foreach (var result in atividadesAgrupadas)
            {
                Labels.Add(result.Key.ToString());
                Values.Add(result.Count);
            }
        }
    }
}
