using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word.Models;
using Novacode;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ADE.GeradorArquivo.Word.Contratos
{
    public class TabelaRegistroDeHoras : ContratoBase
    {
        private string titulo = "FICHA DE REGISTRO DE HORAS DO ESTÁGIO SUPERVISIONADO";
        private List<RegistroDeHoras> DadosTabela;
        private RequisitosBasicosCabecalho Requisitos;
        public TabelaRegistroDeHoras(Cabecalho cabecalho, List<RegistroDeHoras> dados, RequisitosBasicosCabecalho requisitosFichaRegistroHoras) : base(cabecalho)
        {
            DadosTabela = dados;
            Requisitos = requisitosFichaRegistroHoras;
        }

        public override Stream Gerar()
        {
            Stream ms = new MemoryStream();
            using (DocX doc = DocX.Create(ms))
            {
                Cabecalho.Gerar(doc);
                doc.AddTitle(titulo);
                GerarCabecalhoTabela(doc);
                doc.InsertTable(GerarCorpoTabela(doc)).InsertParagraphAfterSelf("").AppendLine();
                doc.InsertParagraph().AppendLine();
                doc.AppendLinhaAssinatura("INSTITUIÇÃO ESTAGIADA");
                doc.AppendLinhaAssinatura("FACISA BH");
                doc.AppendLinhaAssinatura("ESTAGIÁRIO (A)");
                doc.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }

        private void GerarCabecalhoTabela(DocX docX)
        {
            docX.InsertParagraph($"Nome do Aluno (a): {Requisitos.NomeAluno.PadRight(47)} RA: {Requisitos.RA.PadRight(10)}").Alignment = Alignment.left;
            docX.InsertParagraph($"Turma: {Requisitos.Turma.PadRight(17)} Carga Horária do Estágio Supervisionado Exigida: {Requisitos.Carga_Horaria_Exigida.PadRight(4)}").Alignment = Alignment.left;
            docX.InsertParagraph($"Nome da Instituição Estagiada: {Requisitos.Nome_Instituicao}").Alignment = Alignment.left;
        }

        private Table GerarCorpoTabela(DocX docX)
        {
            int rows = DadosTabela.Count;
            int cols = 4;
            Table table = docX.AddTable(rows, cols);

            table.AutoFit = AutoFit.Contents;
            table.Alignment = Alignment.center;

            for (int i = 0; i <= (int)TableBorderType.InsideV; i++)
                table.SetBorder((TableBorderType)i, new Border());

            Formatting Format = new Formatting();
            Format.Bold = true;

            for (int i = 0; i < table.RowCount; i++)
                table.RemoveRow(i);

            table.Rows[0].TableHeader = true;
            table.Rows[0].Cells[0].Paragraphs[0].Bold().InsertText("DATA", true, Format);
            table.Rows[0].Cells[1].Paragraphs[0].Bold().InsertText("INICIO", true, Format);
            table.Rows[0].Cells[2].Paragraphs[0].Bold().InsertText("TÉRMINO", true, Format);
            table.Rows[0].Cells[3].Paragraphs[0].Bold().InsertText("ATIVIDADES DESENVOLVIDAS", true, Format);

            for (int i = 1; i < table.RowCount; i++)
                table.RemoveRow(i);

            for (int i = 1; i <= rows; i++)
            {
                int pos = i - 1;
                Row r1 = table.InsertRow();

                r1.Cells[0].Paragraphs[0].InsertText(DadosTabela[pos].Data.ToShortDateString());
                r1.Cells[1].Paragraphs[0].InsertText(DadosTabela[pos].HoraInicio.ToShortTimeString());
                r1.Cells[2].Paragraphs[0].InsertText(DadosTabela[pos].HoraFim.ToShortTimeString());
                r1.Cells[3].Paragraphs[0].InsertText(DadosTabela[pos].Atividade);
            }

            Row r = table.InsertRow();
            r.Cells[0].Paragraphs[0].Bold().InsertText("Carga Horária Cumprida:", true, Format);
            r.Cells[1].InsertParagraph(($"{DadosTabela.Sum(x => x.CargaHoraria) / 60} horas").ToString());
            r.MergeCells(1, 2);
            return table;
        }
    }
}
