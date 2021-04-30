using ADE.Dominio.Models.Individuais;
using System;
using System.Collections.Generic;
using System.Text;
using ADE.Utilidades.Extensions;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Html;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Testes.UNIT
{
    [Category("HTML")]
    public class TesteExibicaoEntidadeColorCoded
    {
        private readonly ITestOutputHelper output;
        static string color = "#ffc925";
        static string classe = Utilidades.Constants.CSSCustomClassNames.InstituicaoDiv();
        static string nome = "FacisaBH";
        private readonly string MensagemEsperada = $"<div class='{classe}' style=\"border-bottom : 3px solid {color};\"><p class='' >{nome}</p></div>";

        public TesteExibicaoEntidadeColorCoded(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GerarMensagemHTML()
        {
            Instituicao entidade = new Instituicao
                    (nome,
                   @"A Faculdade de Ciências Sociais Aplicadas de Belo Horizonte visa produzir, intermediar e disseminar conhecimento, capacitando desta forma nossos alunos a enfrentarem os diferentes desafios do mercado. Assim sendo, percebe-se que a FACISABH tem recebido o reconhecimento do meio acadêmico e empresarial, em decorrência da qualidade, seriedade e dinamismo dos seus cursos.
 Notoriamente sensível às dificuldades e oportunidades vivenciadas, acreditamos que a busca por novos conhecimentos e aprimoramento dos atuais, por meio de uma educação superior, são essenciais para exercício pleno de qualquer profissão.Desta forma, estruturamos os cursos tendo como objetivo lapidar nossos estudantes, para que estes tenham a correta percepção estratégica e crítica a qual desempenham ou que desempenharão perante a sociedade.
 Temos assim, a plena certeza que, o desenvolvimento intelectual e o aprendizado de ferramentas clássicas e modernas, são condições essenciais para uma jornada de sucesso.Cientes disto, Diretoria, aliado aos nossos Professores doutores, mestres e especialistas, estamos focados em lhe oferecer oportunidades para mudar sua vida.
 Para tanto, procuramos selecionar para nosso quadro estudantil, estudantes ousados, que valorizam a busca por desafios e novos conhecimentos, que possuam um espírito vencedor, que não tenham receio a mudanças, que almejam “degraus” cada vez mais altos na sua carreira, que sejam humildes para reconhecer a necessidade de se qualificarem e se aperfeiçoarem constantemente, sem deixar de ser firmes em suas convicções.",
                    "Antônio Baião de Amorim",
                    "Av. Antônio Carlos, 521 Lagoinha - CEP 31.210 - 010, Belo Horizonte - MG",
                    "9 8978 4750",
                    "ouvidoria@facisa.com.br ",
                    color
                    );

            HtmlString ColorHTML = entidade.ExibirEntidadeColorCoded();
            
            output.WriteLine(ColorHTML.Value);
            Assert.True(ColorHTML.Value == MensagemEsperada);
        }
    }
}
