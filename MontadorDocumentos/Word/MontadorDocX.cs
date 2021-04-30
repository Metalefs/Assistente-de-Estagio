using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word.Fabrica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADE.GeradorArquivo.Word
{
    public class MontadorDocX
    {
        FabricaDocumentos FabricaDocumento;
        public MontadorDocX()
        {
            
        }

        public Stream GerarDocumentoPreenchimento(Documento documento, List<DadosAlunoKV> DadosDocumento, int CodigoGeracao, List<AreaEstagioCurso> areas = null, int IdArea = 0)
        {
            FabricaDocumento = new FabricaDocumentos(documento, CodigoGeracao, areas, IdArea);
            return FabricaDocumento.GerarFormulario(DadosDocumento);
        }

        public Stream GerarDocumentoRegistroHoras(Documento documento, List<RegistroDeHoras> DadosDocumento, RequisitosBasicosCabecalho requisitosFichaRegistroHoras, int CodigoGeracao)
        {
            FabricaDocumento = new FabricaDocumentos(documento, CodigoGeracao);
            return FabricaDocumento.GerarTabela(DadosDocumento, requisitosFichaRegistroHoras);
        }

        public Stream GerarDocumentoQuestionario(Documento documento, List<DadosAlunoKV> DadosDocumento, int CodigoGeracao, RequisitosBasicosCabecalho requisitosBasicos)
        {
            FabricaDocumento = new FabricaDocumentos(documento, CodigoGeracao);
            return FabricaDocumento.GerarQuestionario(DadosDocumento, requisitosBasicos);
        }
    }
}
