using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word.Models;
using System.Collections.Generic;
using System.IO;

namespace ADE.GeradorArquivo.Word.Contratos
{
    public abstract class ContratoBase : IContrato
    {
        protected Cabecalho Cabecalho {get; set;}
        protected List<DadosAlunoKV> Dados {get; set;}
        protected Documento Documento { get; set; }
        public ContratoBase(Cabecalho cabecalho, List<DadosAlunoKV> dados, Documento documento)
        {
            Cabecalho = cabecalho; 
            Dados = dados;
            Documento = documento;
        }
        public ContratoBase(Cabecalho cabecalho)
        {
            Cabecalho = cabecalho;
        }

        public abstract Stream Gerar();
    }
}
