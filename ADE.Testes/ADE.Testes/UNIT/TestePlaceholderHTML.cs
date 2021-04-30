using ADE.Dominio.Models;
using ADE.Utilidades.Extensions;
using System;
using ADE.Utilidades.Handlers;
using Xunit;
using Xunit.Abstractions;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Testes.UNIT
{
    [Category("HTML")]
    public class TestePlaceholderHTML
    {
        private readonly ITestOutputHelper output;
        private readonly string MensagemEsperadaRaw = @"_@D_ _@L_Identificador_/@L_: _@P_1_/@P_ 
 _@L_NomeCurso_/@L_: _@P_Curso teste_/@P_ 
 _@L_Sigla_/@L_: _@P_ADS_/@P_ 
 _@L_CoordenadorCurso_/@L_: _@P_Tester_/@P_ 
 _@L_EmailCoordenadorCurso_/@L_: _@P_teste@ads.com_/@P_ 
 _@L_DescricaoCurso_/@L_: _@P_Entidade teste_/@P_ 
 _@L_TipoCurso_/@L_: _@P_0_/@P_ 
 _@L_Informacao_/@L_: _@P_Teste_/@P_ 
 _@L_Documento_/@L_: _@P_System.Collections.Generic.HashSet`1[ADE.Dominio.Models.Documento]_/@P_ 
 _@L_DataHoraInclusao_/@L_: _@P_01/01/0001 00:00:00_/@P_ 
 _@L_DataHoraUltimaAlteracao_/@L_: _@P_01/01/0001 00:00:00_/@P_ 
_/@D_";
        private readonly string MensagemEsperadaHTML = @"<div class=''> <label class='label h6-responsive'>Identificador</label>: <p class='alteracao-p'>1</p> 
 <label class='label h6-responsive'>NomeCurso</label>: <p class='alteracao-p'>Curso teste</p> 
 <label class='label h6-responsive'>Sigla</label>: <p class='alteracao-p'>ADS</p> 
 <label class='label h6-responsive'>CoordenadorCurso</label>: <p class='alteracao-p'>Tester</p> 
 <label class='label h6-responsive'>EmailCoordenadorCurso</label>: <p class='alteracao-p'>teste@ads.com</p> 
 <label class='label h6-responsive'>DescricaoCurso</label>: <p class='alteracao-p'>Entidade teste</p> 
 <label class='label h6-responsive'>TipoCurso</label>: <p class='alteracao-p'>0</p> 
 <label class='label h6-responsive'>Informacao</label>: <p class='alteracao-p'>Teste</p> 
 <label class='label h6-responsive'>Documento</label>: <p class='alteracao-p'>System.Collections.Generic.HashSet`1[ADE.Dominio.Models.Documento]</p> 
 <label class='label h6-responsive'>DataHoraInclusao</label>: <p class='alteracao-p'>01/01/0001 00:00:00</p> 
 <label class='label h6-responsive'>DataHoraUltimaAlteracao</label>: <p class='alteracao-p'>01/01/0001 00:00:00</p> 
</div>";

        public TestePlaceholderHTML(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GerarMensagemHTML()
        {
            Curso entidade = new Curso()
            {
                Identificador = 1,
                NomeCurso = "Curso teste",
                Sigla = "ADS",
                EmailCoordenadorCurso = "teste@ads.com",
                CoordenadorCurso = "Tester",
                DescricaoCurso = "Entidade teste",
                Informacao = "Teste"
            };
            string MensagemAlteracao = entidade.MensagemDePropriedadesComPlaceholderHTML(String.Empty);
            AlteracaoEntidadesSistema AlteracaoEntidade = new AlteracaoEntidadesSistema()
            {
                IdentificadorEntidade = entidade.Identificador,
                MensagemAlteracao = MensagemAlteracao,
                Autor = new Dominio.Models.Individuais.UsuarioADE(),
                IdAutor = "IdentificadorUsuario",
                Entidade = Dominio.Models.Enums.EnumEntidadesSistema.Curso
            };
            string HtmlParseMessage = TextParser.MontarMensagemHTML(AlteracaoEntidade.MensagemAlteracao);
            output.WriteLine(AlteracaoEntidade.MensagemAlteracao);
            output.WriteLine(HtmlParseMessage);
            Assert.True(MensagemEsperadaRaw.Contains(AlteracaoEntidade.MensagemAlteracao) && MensagemEsperadaHTML == HtmlParseMessage);
        }
        
    }
}
