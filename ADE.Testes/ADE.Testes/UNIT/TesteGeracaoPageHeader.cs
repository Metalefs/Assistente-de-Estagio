using Microsoft.AspNetCore.Html;
using static ADE.Utilidades.Extensions.HTMLExtensions;
using Xunit;
using Xunit.Abstractions;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Testes.UNIT
{
    [Category("HTML")]
    public class TesteGeracaoPageHeader
    {
        private readonly ITestOutputHelper output;
        private readonly string MensagemEsperada = $"<span class=\"card-header modal-header h3-responsive\">Gerenciamento de Documentos</span>";

        public TesteGeracaoPageHeader(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GeraPageHeaderHTML()
        {
            HtmlString ColorHTML = CreatePageHeader("Gerenciamento de Documentos");
            output.WriteLine(ColorHTML.Value);
            Assert.True(ColorHTML.Value == MensagemEsperada);
        }
    }
}
