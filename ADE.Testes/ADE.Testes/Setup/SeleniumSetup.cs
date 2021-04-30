using ADE.Selenium.Helpers;
using ADE.Selenium.Testes;
using Microsoft.AspNetCore.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Selenium.Fixtures
{
    public class SeleniumSetup : IDisposable
    {
        public IWebDriver Driver { get; private set; }
        public IHostingEnvironment _hostingEnvironment { get; private set; }
        
        public SeleniumSetup()
        {
            Driver = new ChromeDriver(Caminhos.PastaDoExecutavel);
        }

        public void AcessarUrl(string URL)
        {
            Driver.Navigate().GoToUrl(URL);
        }

        public void AcessarMenuESubMenu(string NomeMenu, string NomeSubMenu)
        {
            Driver.FindElement(By.Id(NomeMenu)).Click();
            Driver.FindElement(By.Id(NomeSubMenu)).Click();
        }

        public IWebElement ObterElementoPorId(string Id)
        {
            return Driver.FindElement(By.Id(Id));
        }

        public IWebElement ObterElementoPorXPath(string XPath)
        {
            return Driver.FindElement(By.XPath(XPath));
        }

        public string ObterTextoPorId(string Id)
        {
            return Driver.FindElement(By.Id(Id)).Text;
        }

        public string ObterTextoPorXPath(string XPath)
        {
            return Driver.FindElement(By.XPath(XPath)).Text;
        }

        public string ObterValuePorId(string Id)
        {
            return Driver.FindElement(By.Id(Id)).GetProperty("value");
        }

        public void ClicarPorId(string Id)
        {
            Driver.FindElement(By.Id(Id)).Click();
        }

        public void RealizarLogin(string email, string senha)
        {
            var CampoEmail = ObterElementoPorId("campo-email");
            var CampoSenha = ObterElementoPorId("campo-senha");

            CampoEmail.SendKeys(email);
            CampoSenha.SendKeys(senha);

            ClicarPorId("entrarHome");
        }

        public void RealizarLogout()
        {
            ObterElementoPorXPath("//*[@id='sidebar']/ul/li[6]/a").Click();
        }

        public void RealizarCadastro(string email, string senha)
        {
            ClicarPorId("registroHome");

            var CampoEmail = ObterElementoPorId("RegistrarEmail");
            var CampoSenha = ObterElementoPorId("RegistrarPassword");

            CampoEmail.SendKeys("admin@assistentedeestagio.com");
            CampoSenha.SendKeys("123");
            ClicarPorId("completar-cadastro");
        }

        public void Fechar()
        {
            Driver.Close();
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }
}
