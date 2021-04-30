using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word;
using Novacode;
using System.Collections.Generic;
using System.IO;

namespace ADE.GeradorArquivo.Word.Contratos
{
    public class Questionario : ContratoBase
    {
        RequisitosBasicosCabecalho Requisitos;
        public Questionario(Documento documento, Cabecalho cabecalho, List<DadosAlunoKV> dados, RequisitosBasicosCabecalho requisitos) : base(cabecalho,dados,documento)
        {
            Requisitos = requisitos;
        }

        public override Stream Gerar()
        {
            Stream ms = new MemoryStream();
            string titulo = Documento.TituloDocumento;
            if(Documento.Arquivo != null)
            {
                ms = new MemoryStream(Documento.Arquivo);
                DocX doc = DocX.Load(ms);
                doc.SubstituirCamposDocumento(Dados);
                doc.SaveAs(ms);
            }
            else
            {
                using (DocX doc = DocX.Create(ms))
                {
                    Cabecalho.Gerar(doc);
                    doc.AddTitle(titulo);
                    GerarCabecalhoTabela(doc);
                    GerarSubtitulo(doc);
                    GerarCorpoDocumento(doc);
                    GerarCamposAdicionais(doc);
                    doc.SaveAs(ms);
                }
            }
            ms.Position = 0;
            return ms;
        }

        private void GerarCabecalhoTabela(DocX docX)
        {
            Paragraph NomeRA = docX.InsertParagraph($"Nome do Aluno(a): {Requisitos.NomeAluno.PadRight(47)} RA: {Requisitos.RA.PadRight(10)}");
            NomeRA.SetLineSpacing(LineSpacingType.Line, 1.5f);
            NomeRA.IndentationBefore = 2f;
            Paragraph TurmaCargaHoraria = docX.InsertParagraph($"Turma: {Requisitos.Turma.PadRight(17)} Carga Horária do Estágio Supervisionado Exigida: {Requisitos.Carga_Horaria_Exigida.PadRight(4)}");
            TurmaCargaHoraria.SetLineSpacing(LineSpacingType.Line, 1.5f);
            TurmaCargaHoraria.IndentationBefore = 2f;
            Paragraph NomeInstituicao = docX.InsertParagraph($"Nome da Instituição Estagiada: {Requisitos.Nome_Instituicao.PadRight(30)}");
            NomeInstituicao.SetLineSpacing(LineSpacingType.Line, 1.5f);
            NomeInstituicao.IndentationBefore = 2f;
            Paragraph Area = docX.InsertParagraph($"Área específica: {Requisitos.AreaEstagio.PadRight(30)}").AppendLine();
            Area.SetLineSpacing(LineSpacingType.Line, 1.5f);
            Area.IndentationBefore = 2f;
        }

        private void GerarSubtitulo(DocX docX)
        {
            docX.InsertParagraph("Questionário Avaliativo Dissertativo").Bold().FontSize(12).Alignment = Alignment.center;
            docX.InsertParagraph("(Deve ser entregue pelo aluno no término do Estágio Supervisionado) ").FontSize(10).Alignment = Alignment.center;
            docX.InsertParagraph(System.Environment.NewLine).AppendLine();
        }

        public void GerarCorpoDocumento(DocX documento)
        {
            int i = 1;
            foreach (DadosAlunoKV dado in Dados)
            {
                string Pergunta = dado.Requisito.NomeRequisito;
                string Resposta = dado.Value;

                Paragraph pergunta = documento.InsertParagraph(i+") " + Pergunta).Bold().AppendLine();

                pergunta.Font("Arial");
                pergunta.FontSize(12);
                pergunta.IndentationBefore = 2f;

                Paragraph resposta = documento.InsertParagraph(Resposta).AppendLine();

                resposta.Font("Arial");
                resposta.FontSize(12);
                resposta.IndentationBefore = 2f;
                i++;
            }
        }

        public void GerarCamposAdicionais(DocX doc)
        {
            if (Documento.PossuiData)
            {
                doc.AppendLinhaData();
            }
            if (Documento.PossuiCarimboCNPJ)
            {
                doc.AppendTabelaCarimbo();
            }
            if (Documento.PossuiAssinaturaResposavelEstagio)
            {
                doc.AppendLinhaAssinatura(Documento.Assinatura.EnumDescription());
            }
        }

    }
}
