using Microsoft.AspNetCore.Html;
using static ADE.Utilidades.Helpers.PropertyGenerator;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using static ADE.Utilidades.Handlers.TextParser;
using ADE.Utilidades.Constants;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Testes.UNIT
{
    [Category("HTML")]
    public class TesteGeracaoMultiplasPropriedadesHTML
    {
        private readonly ITestOutputHelper output;
        string Name = "Elementoname";
        string ValorAtributo = "Elementovalue";
        string NomeAtributo = "ElementoName";
        private readonly string MensagemEsperada = "<div  class=\"md-form\" > <label  for=\"Elementoname\" >ElementoName</label>: <input  id=\"Elementoname\"  name=\"Elementoname\"  value=\"Elementovalue\"  class=\"form-control\"  placeholder=\"ElementoName\"  type=\"text\"  /> </div>";

        public TesteGeracaoMultiplasPropriedadesHTML(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GerarElementoComMultiplosAtributosHTML()
        {
            List<KeyValuePair<string, string>> Styles = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("id", $"{Name}"),
                new KeyValuePair<string, string>("name", $"{Name}"),
                new KeyValuePair<string, string>("value", $"{ValorAtributo}"),
                new KeyValuePair<string, string>("class", $"form-control"),
                new KeyValuePair<string, string>("placeholder", $"{NomeAtributo}"),
                new KeyValuePair<string, string>("type", $"text")
            };
            string For = CreateHTMLProperty("for", Name);
            string Propriedades = CreateHTMLProperties(Styles);
            string Div = AninharEmDiv(string.Format(" {0}: {1} ", AninharEmLabel(NomeAtributo, For), HTMLElements.Input(Propriedades)), CreateHTMLProperty("class", "md-form"));
            output.WriteLine(Div);
            Assert.True(Div == MensagemEsperada);
        }
    }
}
