﻿using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Utilidades.Extensions;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ADE.Testes.UNIT
{
    [Category("HTML")]
    public class TesteGeracaoListaDePropriedadesParaEntidadeHTML
    {
        private readonly ITestOutputHelper output;
        private readonly string MensagemEsperada = "<div class='md-form'  title=''><div class=''  title=''> <label class=''  title=''>Nome do Curso</label>: <p class=''  title='NomeCurso'>Análise e Desenvolvimento de Sistemas</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>Sigla do Curso</label>: <p class=''  title='Sigla'>ADS</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>Coordenador do Curso</label>: <p class=''  title='CoordenadorCurso'>André rodrigues</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>E-mail do Coordenador do Curso</label>: <p class=''  title='EmailCoordenadorCurso'>andre.augusto.rodrigues@hotmail.com</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>Descrição do Curso</label>: <p class=''  title='DescricaoCurso'>Analisa, projeta, desenvolve, testa, implanta e mantém sistemas computacionais de informação. Avalia, seleciona, especifica e utiliza metodologias, tecnologias e ferramentas da Engenharia de Software, linguagens de programação e bancos de dados. Coordena equipes de produção de softwares. Vistoria, realiza perícia, avalia, emite laudo e parecer técnico em sua área de formação.</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>Tipo de Curso</label>: <p class=''  title='TipoCurso'>Tecnologo</p> </div>\r\n<div class=''  title=''> <label class=''  title=''>Instituição</label>: <p class=''  title='IdInstituicao'>0</p> </div>\r\n</div>";
        
        public TesteGeracaoListaDePropriedadesParaEntidadeHTML(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GerarListaPropriedadesHTML()
        {
            Instituicao InstituicaoZero = new Instituicao
                    ("FacisaBH",
                   @"A Faculdade de Ciências Sociais Aplicadas de Belo Horizonte visa produzir, intermediar e disseminar conhecimento, capacitando desta forma nossos alunos a enfrentarem os diferentes desafios do mercado. Assim sendo, percebe-se que a FACISABH tem recebido o reconhecimento do meio acadêmico e empresarial, em decorrência da qualidade, seriedade e dinamismo dos seus cursos.
 Notoriamente sensível às dificuldades e oportunidades vivenciadas, acreditamos que a busca por novos conhecimentos e aprimoramento dos atuais, por meio de uma educação superior, são essenciais para exercício pleno de qualquer profissão.Desta forma, estruturamos os cursos tendo como objetivo lapidar nossos estudantes, para que estes tenham a correta percepção estratégica e crítica a qual desempenham ou que desempenharão perante a sociedade.
 Temos assim, a plena certeza que, o desenvolvimento intelectual e o aprendizado de ferramentas clássicas e modernas, são condições essenciais para uma jornada de sucesso.Cientes disto, Diretoria, aliado aos nossos Professores doutores, mestres e especialistas, estamos focados em lhe oferecer oportunidades para mudar sua vida.
 Para tanto, procuramos selecionar para nosso quadro estudantil, estudantes ousados, que valorizam a busca por desafios e novos conhecimentos, que possuam um espírito vencedor, que não tenham receio a mudanças, que almejam “degraus” cada vez mais altos na sua carreira, que sejam humildes para reconhecer a necessidade de se qualificarem e se aperfeiçoarem constantemente, sem deixar de ser firmes em suas convicções.",
                    "Antônio Baião de Amorim",
                    "Av. Antônio Carlos, 521 Lagoinha - CEP 31.210 - 010, Belo Horizonte - MG",
                    "9 8978 4750",
                    "ouvidoria@facisa.com.br ",
                    "#ffc925"
                    );

            Curso curso = new Curso()
            {
                NomeCurso = "Análise e Desenvolvimento de Sistemas",
                Sigla = "ADS",
                DescricaoCurso = "Analisa, projeta, desenvolve, testa, implanta e mantém sistemas computacionais de informação. Avalia, seleciona, especifica e utiliza metodologias, tecnologias e ferramentas da Engenharia de Software, linguagens de programação e bancos de dados. Coordena equipes de produção de softwares. Vistoria, realiza perícia, avalia, emite laudo e parecer técnico em sua área de formação.",
                CoordenadorCurso = "André rodrigues",
                EmailCoordenadorCurso = "andre.augusto.rodrigues@hotmail.com",
                Instituicao = InstituicaoZero,
                TipoCurso = Dominio.Models.Enums.EnumTipoCurso.Tecnologo
            };

            HtmlString HTML = curso.MensagemDePropriedadesEmHTML("md-form");

            output.WriteLine(HTML.Value);
            Assert.True(HTML.Value == MensagemEsperada);
        }
    }
}
