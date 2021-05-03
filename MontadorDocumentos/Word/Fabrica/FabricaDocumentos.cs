using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word.Contratos;
using ADE.GeradorArquivo.Word.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADE.GeradorArquivo.Word.Fabrica
{
    public class FabricaDocumentos
    {
        private IContrato FormularioEstagio;
        private IContrato Tabela;

        private Cabecalho Cabecalho;
        private Documento Documento;
        private List<AreaEstagioCurso> Areas;
        private int IdArea;

        public FabricaDocumentos(Documento documento, int CodigoGeracao, List<AreaEstagioCurso> areas = null, int idArea = 0)
        {
            Cabecalho = new Cabecalho(documento, CodigoGeracao);
            Documento = documento;
            Areas = areas;
            IdArea = idArea;
        }

        public Stream GerarFormulario(List<DadosAlunoKV> DadosAluno) 
        {
            FormularioEstagio = new Formulario(Documento, Cabecalho,  DadosAluno, Areas, IdArea);
            return FormularioEstagio.Gerar();
        }

        public Stream GerarTabela(List<RegistroDeHoras> DadosAluno, RequisitosBasicosCabecalho requisitosFichaRegistroHoras)
        {
            Tabela = new TabelaRegistroDeHoras(Cabecalho, DadosAluno, requisitosFichaRegistroHoras);
            return Tabela.Gerar();
        }

        public Stream GerarQuestionario(List<DadosAlunoKV> DadosAluno, RequisitosBasicosCabecalho requisitosBasicos)
        {
            FormularioEstagio = new Questionario(Documento, Cabecalho, DadosAluno, requisitosBasicos);
            return FormularioEstagio.Gerar();
        }

    }
}
