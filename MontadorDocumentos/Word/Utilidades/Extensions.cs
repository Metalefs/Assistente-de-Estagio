using ADE.Dominio.Models;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ADE.GeradorArquivo.Word
{
    public static class Extensions
    {
        public static void AddPicture(this DocX document, Stream Logo, int h, int w)
        {
            var streamImage = document.AddImage(Logo);
            var pictureStream = streamImage.CreatePicture(h, w);
            Paragraph p3 = document.InsertParagraph();
            p3.AppendPicture(pictureStream);
        }
        
        public static void AddTitle(this DocX document, string Titulo)
        {
            document.InsertParagraph(Titulo).FontSize(15d).CapsStyle(CapsStyle.caps).SpacingBefore(10d).SpacingAfter(25d).Alignment = Alignment.center;
        }

        public static void AdicionarCodigoDeGeracao(this DocX document, int codigo)
        {
            if (document.Bookmarks["COD_GERACAO"] != null)
            {
                document.Bookmarks["COD_GERACAO"].SetText(codigo.ToString());
            }
        }

        public static void AppendLinhaAssinatura(this DocX document, string responsavel, int margin = 10)
        {
            int TamanhoCampoAssinatura = 48;
            char PadChar = '_';

            Paragraph p = document.InsertParagraph("".PadRight(TamanhoCampoAssinatura, PadChar)).AppendLine();
            p.Alignment = Alignment.center; 
            
            Paragraph p2 = document.InsertParagraph(responsavel).AppendLine();
            p2.Alignment = Alignment.center;            
        }

        public static void AppendLinhaData(this DocX document)
        {
            int TamanhoCampoCidade = 22;
            int TamanhoCampoDia = 5;
            int TamanhoCampoMes = 20;
            int TamanhoCampoAno = 10;
            char PadChar = '_';

            StringBuilder sb = new StringBuilder(0);
            sb.Append("".PadRight(TamanhoCampoCidade, PadChar));
            sb.Append(", ");
            sb.Append("".PadRight(TamanhoCampoDia, PadChar));
            sb.Append(" de ");
            sb.Append("".PadRight(TamanhoCampoMes, PadChar));
            sb.Append("de");
            sb.Append("".PadRight(TamanhoCampoAno, PadChar));
            sb.Append(".");

            document.InsertParagraph(sb.ToString()).SpacingBefore(10d).SpacingAfter(35d).Alignment = Alignment.center;
        }

        public static void AppendTabelaCarimbo(this DocX document)
        {
            Table t = document.AddTable(1, 1);
            t.Design = TableDesign.TableNormal;
            t.Alignment = Alignment.center;
            t.Rows[0].Cells[0].InsertParagraph("Carimbo do CNPJ").SpacingBefore(10d).SpacingAfter(25d);
        }

        public static void SubstituirCamposDocumento(this DocX document, List<DadosAlunoKV> dadosAluno)
        {
            foreach (DadosAlunoKV dados in dadosAluno)
            {
                if (document.Bookmarks[dados.Requisito.Bookmark] != null)
                {
                    document.Bookmarks[dados.Requisito.Bookmark].SetText(dados.Value);
                }
            }
        }
        public static void GerarTabelaAreaDeEstagio(this DocX document, List<AreaEstagioCurso> areas, int IdArea)
        {
            int rows = areas.Count();
            Table t = document.AddTable(rows, 2);

            Formatting format = new Formatting();
            format.Bold = true;
            t.Paragraphs.ForEach(x => x.FontSize(12));
            t.Paragraphs.ForEach(x => x.Font("Arial"));
            t.Alignment = Alignment.center;
            t.AutoFit = AutoFit.ColumnWidth;
            for (int i = 0; i <= (int)TableBorderType.InsideV; i++)
                t.SetBorder((TableBorderType)i, new Border());

            for (int i = 1; i <= rows; i++)
            {
                int pos = i - 1;
                if (i % 7 == 0)
                    document.InsertSectionPageBreak();
                
                t.Rows[pos].Cells[1].Paragraphs[0].InsertText(areas[pos].Nome);
                if(areas[pos].Identificador == IdArea)
                    t.Rows[pos].Cells[0].Paragraphs[0].InsertText("X",false, format);
            }

            document.InsertTable(t);
        }
    }
}
