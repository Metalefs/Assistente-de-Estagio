/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using Selenium.Utils;
using TesteADE;
using Assistente_de_Estagio.Testes;
using Assistente_de_Estagio.Models;

namespace Assistente_de_Estagio.Testes
{
    public class TestesADE
    {
        private IConfiguration _configuration;

        public TestesADE()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            _configuration = builder.Build();
        }

        [Theory]
        [InlineData(Browser.Firefox, "Vinicius")]
        [InlineData(Browser.Firefox, "Fernando")]
        [InlineData(Browser.Firefox, "Gustavo")]
        [InlineData(Browser.Chrome, "Robson")]
        [InlineData(Browser.Chrome, "Guilherme")]
        [InlineData(Browser.Chrome, "Metalefs")]
        public void TestarPreenchimento(
            Browser browser, string dados)
        {
            TelaPreenchimento tela =
                new TelaPreenchimento(_configuration, browser);

            tela.CarregarPagina();
            tela.PreencherCampos(dados);
            //tela.ProcessarConversao();
            string resultado = tela.ObterCampo();
            tela.Fechar();

            Assert.Equal(dados, resultado);
        }
    
    }
}
*/