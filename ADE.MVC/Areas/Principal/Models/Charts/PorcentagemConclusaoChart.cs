using Assistente_de_Estagio.Areas.Administracao.Models.Charts;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;
using System;

namespace Assistente_de_Estagio.Areas.Principal.Models.Charts
{
    public class PorcentagemConclusaoChart : BaseChart, IChart
    {
        public float ProgressoUsuario { get; set; }
        public float Total { get; set; }

        public PorcentagemConclusaoChart(float progressoUsuario, float total)
        {
            ProgressoUsuario = progressoUsuario;
            Total = total;
        }

        public override void Parse()
        {
            throw new InvalidOperationException();
        }
    }
}
