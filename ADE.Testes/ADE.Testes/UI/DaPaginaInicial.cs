using ADE.Selenium.Fixtures;
using ADE.Selenium.Helpers;
using Microsoft.AspNetCore.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace ADE.Selenium.Testes
{
    public class DaPaginaInicial
    {
        public SeleniumSetup Robo { get; private set; }
        public IHostingEnvironment _hostingEnvironment { get; private set; }

        public DaPaginaInicial()
        {
            Robo = new SeleniumSetup();
        }

        [Fact]
        public void DadoChromeAbertoDeveMostrarEntrarNoTitulo()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Assert.Contains("Entrar - Assistente de Est�gio", Robo.Driver.Title);
            Robo.Fechar();
        }

        [Fact]
        public void DadoChromeAbertoDeveMostrarAsssitenteDeEstagio()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            string TituloCartao = Robo.ObterTextoPorId("tituloHome");
            Assert.Contains("Assistente de Est�gio", TituloCartao);
            Robo.Fechar();
        }

    }
}
