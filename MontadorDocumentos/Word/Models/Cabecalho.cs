using ADE.Dominio.Models;
using Novacode;
using System;
using System.IO;
using System.Linq;

namespace ADE.GeradorArquivo.Word
{
    public class Cabecalho
    {
        public string Titulo { get; set; }
        public Stream Logo { get; set; }
        public Curso Curso { get; set; }
        public int CodigoGeracao { get; set; }

        public Cabecalho(Documento documento, int codigoGeracao)
        {
            Logo = new MemoryStream(documento.IdCursoNavigation.Instituicao.Logo);
            Curso = documento.IdCursoNavigation;
            CodigoGeracao = codigoGeracao;
            if (Curso.Instituicao == null) throw new Exception("Curso sem instituicao!");
        }

        public void Gerar(DocX document)
        {
            document.MarginTop = 0F;
            document.MarginRight = 0F;
            document.MarginBottom = 0F;
            document.MarginLeft = 0F;
            document.AddHeaders();
            document.AddFooters();
            var headerDefault = document.Headers.odd;
            var footerDefault = document.Footers.odd;

            Table t = headerDefault.InsertTable(1, 2);
            Logo.Position = 0;
            Image image = document.AddImage(Logo);
            Picture picture = image.CreatePicture(100, 100);
            
            t.Alignment = Alignment.center;
            t.AutoFit = AutoFit.Contents;
            //for (int i = 0; i <= (int)TableBorderType.InsideV; i++)
            //    t.SetBorder((TableBorderType)i, new Border());

            t.Rows[0].Cells[0].Paragraphs[0].InsertPicture(picture).Alignment = Alignment.center;
            ObterInformacoesCabecalho(t);
            t.Paragraphs.RemoveAll(x => x.Text != null || x.Text != "");
            t.InsertParagraphAfterSelf("").AppendLine();
            document.MarginRight = 15F;
            document.MarginLeft = 30F;

        }

        public Picture Picture(DocX document)
        {
            Logo.Position = 0;
            Image image = document.AddImage(Logo);
            return image.CreatePicture(100, 100);
        }

        private void ObterInformacoesCabecalho(Table table)
        {
            Paragraph informacaoDocumento = table.Rows[0].Cells[1].Paragraphs.First().FontSize(10);
           
            informacaoDocumento.Font("Arial").Alignment = Alignment.left;
            informacaoDocumento.AppendLine("Nucleo de Estágio - "+Curso.Instituicao.ToString()).Bold().FontSize(12);
            informacaoDocumento.AppendLine(Curso.Instituicao.Endereco + System.Environment.NewLine);
            informacaoDocumento.Append($"Fone: ");
            informacaoDocumento.AppendLine($"{Curso.Instituicao.Telefone} /").Append($" {Curso.Instituicao.Website} {System.Environment.NewLine}").UnderlineStyle(UnderlineStyle.dash);
            informacaoDocumento.Append($"Email: ");
            informacaoDocumento.AppendLine($"{Curso.EmailCoordenadorCurso}");
        }
    }
}
