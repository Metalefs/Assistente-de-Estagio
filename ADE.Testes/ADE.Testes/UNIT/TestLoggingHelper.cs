using Xunit;
using Xunit.Abstractions;
using ADE.Utilidades.Handlers;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using System;

namespace ADE.Testes.UNIT
{
    public class TestLoggingHelper
    {
        private readonly ITestOutputHelper output;
        private TipoEvento evento = TipoEvento.Criacao;
        private string MensagemEsperada = "L Taylor (ID: id-teste) CRIOU Documento 'Declaração de Opção pela Área de Estágio' - (Preenchimento - Aluno)";
        
        public TestLoggingHelper(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GerarMensagemDeLog()
        {
            output.WriteLine(ObterMensagemCriacaoCursoDocumento());
            Assert.True(MensagemEsperada == ObterMensagemCriacaoCursoDocumento());
        }
        
        private string ObterMensagemCriacaoCursoDocumento()
        {
            UsuarioADE taylor = ObterUsuarioTaylor();
            Documento declaracao = ObterDocumentoDeclaracao();
            return LoggingHandler<Documento>.GerarMensagemTipoLog(taylor,declaracao,evento);
        }

        public Documento ObterDocumentoDeclaracao()
        {
            Documento doc = new Documento(1, "Declaração de Opção pela Área de Estágio", "No Description Available", 1);
            doc.Tipo = EnumTipoDocumento.Preenchimento;
            doc.Assinatura = EnumAssinaturaDocumento.Aluno;
            return doc;
        }

        private UsuarioADE ObterUsuarioTaylor()
        {
            UsuarioADE Taylor = new UsuarioADE();
            Taylor.UserName = "L Taylor";
            Taylor.Id = "id-teste";
            return Taylor;
        }
    }
}
