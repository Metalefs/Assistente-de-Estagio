using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using System.Collections.Generic;
using System.Linq;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public class TrafegoSite : BaseChart, IChart
    {
        private List<Logins> logins;
        public TrafegoSite(List<Logins> _logins)
        {
            Labels = new List<string>();
            Values = new List<int>();
            logins = _logins;
            Parse();
        }

        public override void Parse()
        {
            var loginsAgrupados = logins.GroupBy(
               dataLogin => dataLogin.DataHoraLogin.Date,
               id => id.Identificador,
               (dataLogin, id) => new
               {
                   Key = dataLogin,
                   Count = id.Count()
               });

            foreach (var result in loginsAgrupados)
            {
                Labels.Add(result.Key.ToString());
                Values.Add(result.Count);
            }
        }
    }
}
