using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Administracao.Models.Charts.Interface
{
    public interface IChart
    {
        List<string> Labels { get; set; }
        List<int> Values { get; set; }
        void Parse();
    }
}
