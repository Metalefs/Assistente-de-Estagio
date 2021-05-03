using ADE.Dominio.Interfaces.Testes;
using ADE.Selenium.Fixtures;
using ADE.Selenium.Helpers;
using OpenQA.Selenium;
using System.Threading;
using Xunit;

namespace ADE.Selenium.Testes
{
    public class DoLogin : ILogin
    {
        public SeleniumSetup Robo { get; private set; }

        public DoLogin()
        {
            Robo = new SeleniumSetup();
        }

        [Fact]
        public void LoginComDadosInvalidos()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarLogin(EmailInvalido,SenhaInvalida);
            
            string IsEmailValid = Robo.ObterElementoPorId("campo-email").GetAttribute("aria-invalid");
            
            Assert.True("false" == IsEmailValid);
            Robo.Fechar();
        }

        [Fact]
        public void LoginComCombinacaoEmailSenhaErrada()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarLogin(EmailValido, SenhaInvalida);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");

            Assert.True("Falha ao autenticar o usuário, você confirmou o registro por E-mail?" == TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public void LoginComOmissaoDeSenha()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarLogin(EmailValido, "");

            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");

            Assert.Contains("Os campos de login não podem estar vazios", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public void LoginComOmissaoDeEmail()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarLogin("", SenhaValida);

            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");

            Assert.Contains("Os campos de login não podem estar vazios", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public void LoginComDadosValidos()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarLogin(EmailValido, SenhaValida);

            Thread.Sleep(5000);
            string TituloCurso = Robo.Driver.FindElement(By.ClassName("tituloInstrucao")).Text;
            Assert.True(TituloCurso.Length > 0);

            Robo.RealizarLogout();
            Robo.Fechar();
        }
    }
}
