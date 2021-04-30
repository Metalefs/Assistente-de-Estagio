using ADE.Selenium.Helpers;
using ADE.Testes.Constants;
using System.IO;
using Xunit;
using ADE.Dominio.Models;
using Xunit.Abstractions;
using ADE.Utilidades.Handlers;
using ADE.Dominio.Models.Individuais;

namespace ADE.Testes.UNIT
{

    public class TestServicoDocumento
    {
        private readonly ITestOutputHelper output;
        
        public TestServicoDocumento(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AoFazerUploadDeDocXArquivoDeveTerMimeCorretoEMenosDeDoisMB()
        {
            FileInfo ArquivoValido = ObterArquivoTeste(Nomes.ArquivoDocXValido);
            FileInfo ArquivoInvalido = ObterArquivoTeste(Nomes.ArquivoDocXInvalido);

            Assert.True(true == ValidaMimeTypeArquivo(ArquivoValido));
            Assert.True(true == ValidaTamanhoArquivo(ArquivoValido));

            Assert.False(true == ValidaMimeTypeArquivo(ArquivoInvalido));
            Assert.False(true == ValidaTamanhoArquivo(ArquivoInvalido));
        }

        [Fact]

        public void TestGeracaoCaminhoDocumento()
        {
            string result = ObterCaminhoGeradoParaDocumentoTeste();
            string Expected = "C:\\Users\\User\\Source\\Repos\\Assistente De Estágio4\\ADE.MVC\\wwwroot\\Documents\\FacisaBH\\ADS\\";
            output.WriteLine(result +"\n"+ Expected);
            Assert.True(result == Expected);
        }

        [Fact]
        public void TestCriacaoDocumento()
        {
            var filePath = ObterCaminhoGeradoParaDocumentoTeste();
            Directory.CreateDirectory(filePath);
            
            output.WriteLine(filePath);
            Assert.True(true == Directory.Exists(filePath));
        }

        private string ObterCaminhoGeradoParaDocumentoTeste()
        {
            Documento documento = new Documento(1, "DocumentoTeste", "DescTeste", 1);
            Instituicao instituicao = new Instituicao
                    ("FacisaBH",
                   @"A Faculdade de Ciências Sociais Aplicadas de Belo Horizonte visa produzir, intermediar e disseminar conhecimento, capacitando desta forma nossos alunos a enfrentarem os diferentes desafios do mercado. Assim sendo, percebe-se que a FACISABH tem recebido o reconhecimento do meio acadêmico e empresarial, em decorrência da qualidade, seriedade e dinamismo dos seus cursos.
 Notoriamente sensível às dificuldades e oportunidades vivenciadas, acreditamos que a busca por novos conhecimentos e aprimoramento dos atuais, por meio de uma educação superior, são essenciais para exercício pleno de qualquer profissão.Desta forma, estruturamos os cursos tendo como objetivo lapidar nossos estudantes, para que estes tenham a correta percepção estratégica e crítica a qual desempenham ou que desempenharão perante a sociedade.
 Temos assim, a plena certeza que, o desenvolvimento intelectual e o aprendizado de ferramentas clássicas e modernas, são condições essenciais para uma jornada de sucesso.Cientes disto, Diretoria, aliado aos nossos Professores doutores, mestres e especialistas, estamos focados em lhe oferecer oportunidades para mudar sua vida.
 Para tanto, procuramos selecionar para nosso quadro estudantil, estudantes ousados, que valorizam a busca por desafios e novos conhecimentos, que possuam um espírito vencedor, que não tenham receio a mudanças, que almejam “degraus” cada vez mais altos na sua carreira, que sejam humildes para reconhecer a necessidade de se qualificarem e se aperfeiçoarem constantemente, sem deixar de ser firmes em suas convicções.",
                    "Antônio Baião de Amorim",
                    "Av. Antônio Carlos, 521 Lagoinha - CEP 31.210 - 010, Belo Horizonte - MG",
                    "9 8978 4750",
                    "ouvidoria@facisa.com.br",
                    "#ffc925"
                    );
            Curso Curso = new Curso()
            {
                NomeCurso = "Análise e Desenvolvimento de Sistemas",
                Sigla = "ADS",
                DescricaoCurso = "Analisa, projeta, desenvolve, testa, implanta e mantém sistemas computacionais de informação. Avalia, seleciona, especifica e utiliza metodologias, tecnologias e ferramentas da Engenharia de Software, linguagens de programação e bancos de dados. Coordena equipes de produção de softwares. Vistoria, realiza perícia, avalia, emite laudo e parecer técnico em sua área de formação.",
                CoordenadorCurso = "André rodrigues",
                EmailCoordenadorCurso = "andre.augusto.rodrigues@hotmail.com",
                Instituicao = instituicao
            };
            return FileHandler.GerarCaminhoDiretorioDocumento(instituicao, Curso, documento, Caminhos.RootTeste);
        }

        private bool ValidaMimeTypeArquivo(FileInfo arquivo)
        {
            output.WriteLine(arquivo.Extension);
            if (FileHandler.ArquivoDocXValido(arquivo))
                return true;
            return false;
        }
        
        private bool ValidaTamanhoArquivo(FileInfo arquivo)
        {
            output.WriteLine(arquivo.Length.ToString());
            if (arquivo.Length <= Tamanhos.TamanhoMaximoDocX)
                return true;
            return false;
        }

        private FileInfo ObterArquivoTeste(string NomeArquivo)
        {
            string CaminhoTeste = Caminhos.RootTeste;
            DirectoryInfo TestDir = new DirectoryInfo(CaminhoTeste);
            FileInfo[] arquivos = TestDir.GetFiles();
            foreach(var arquivo in arquivos)
            {
                if (arquivo.Name == NomeArquivo)
                    return arquivo;            
            }
            throw new FileNotFoundException();
        }
    }
}
