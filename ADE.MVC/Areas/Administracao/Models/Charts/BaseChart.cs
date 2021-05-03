using System.Collections.Generic;
using Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts
{
    public abstract class BaseChart : IChart
    {
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }
        public abstract void Parse();
    }
}
