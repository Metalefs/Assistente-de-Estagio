/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assistente_de_Estagio;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.Utils;
using TesteADE;
namespace Assistente_de_Estagio.Models
{
    public class TelaPreenchimento
    {
        private IConfiguration _configuration;
        private Browser _browser;
        private IWebDriver _driver;

        public TelaPreenchimento(
            IConfiguration configuration, Browser browser)
        {
            _configuration = configuration;
            _browser = browser;

            string caminhoDriver = null;
            if (browser == Browser.Firefox)
            {
                caminhoDriver =
                    _configuration.GetSection("Selenium:CaminhoDriverFirefox").Value;
            }
            else if (browser == Browser.Chrome)
            {
                caminhoDriver =
                    _configuration.GetSection("Selenium:CaminhoDriverChrome").Value;
            }

            _driver = WebDriverFactory.CreateWebDriver(
                browser, caminhoDriver);
        }
        public void CarregarPagina()
        {
            _driver.LoadPage(
                TimeSpan.FromSeconds(5),
                _configuration.GetSection("Selenium:UrlTelaConversaoDistancias").Value);
        }

        public void PreencherCampos(string valor)
        {
            _driver.SetText(
                By.Id("_Nd1A_"),
                valor.ToString());
        }

        public void ProcessarConversao()
        {
            _driver.Submit(By.Id("btnConverter"));

            var wait = new WebDriverWait(
                _driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => d.FindElement(By.Id("DistanciaKm")) != null);
        }

        public string ObterCampo()
        {
            return _driver.GetText(By.Id("_Nd1A_"));
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }

    }

}*/
