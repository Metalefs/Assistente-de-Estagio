using ADE.Dominio.Interfaces.Testes;
using ADE.Dominio.Models.Individuais;
using ADE.Selenium.Fixtures;
using ADE.Selenium.Helpers;
using Microsoft.AspNetCore.Identity;
using OpenQA.Selenium;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ADE.Selenium.Testes
{
    public class DoCadastro : ICadastro
    {
        public SeleniumSetup Robo { get; private set; }
        
        public DoCadastro()
        {
            Robo = new SeleniumSetup();
        }

        //private async Task ReiniciarTeste()
        //{
        //    UsuarioADE usuario = await userManager.FindByEmailAsync(EmailValido);
        //    await userManager.DeleteAsync(usuario);
        //}

        [Fact]
        public override void CadastroComEmailInvalido()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarCadastro(EmailInvalido,SenhaInvalida);
            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");
            Assert.Contains("Senhas devem conter ao menos 6 caracteres.", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public override void CadastroComEmailEmUso()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarCadastro(EmailEmUso, SenhaValida);
            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");
            Assert.Contains("", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public override async Task CadastroComSenhaInvalida()
        {
            //await ReiniciarTeste();
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarCadastro(EmailValido, SenhaInvalida);
            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");
            Assert.Contains("", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public override void CadastroComOmissaoDeDados()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarCadastro("","");
            Thread.Sleep(5000);

            string TextoErro = Robo.ObterTextoPorId("Campo-Validacao");
            Assert.Contains("", TextoErro);
            Robo.Fechar();
        }

        [Fact]
        public override void CadastroComDadosValidos()
        {
            Robo.AcessarUrl(Caminhos.StartUrl);

            Robo.RealizarCadastro(EmailValido, SenhaValida);
            Thread.Sleep(5000);

            Assert.True(CadastroComSucesso());
            Robo.Fechar();
        }

        private bool CadastroComSucesso()
        {
            string TituloCurso = Robo.ObterTextoPorId("titulo-selecao-curso-inicial");
            return (TituloCurso == "Cursos");
        }
    }
}
