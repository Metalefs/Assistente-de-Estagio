using ADE.Dominio.Models;
using static ADE.Utilidades.Handlers.TextParser;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADE.Utilidades.Helpers;

namespace ADE.GeradorArquivo.Word
{
    public class UtilidadesDocX
    {
        public string ObterTextoEmHTML(Stream stream)
        {
            DocX doc = DocX.Load(stream);
            IList<Paragraph> Paragrafos = doc.Paragraphs;
            StringBuilder Texto = new StringBuilder();

            foreach (Paragraph p in Paragrafos)
            {
                IEnumerable<Bookmark> Bookmarks = p.GetBookmarks();
                foreach (Bookmark b in Bookmarks)
                {
                    string propriedade = PropertyGenerator.CreateHTMLProperty("class","text-primary");
                    string text = AninharEmElemento("p", b.Name, propriedade);
                    b.Paragraph.InsertAtBookmark($" {text} ", b.Name);
                }
                Texto.AppendLine($" {p.Text}");
            }
            return Texto.ToString();
        }
        public string ObterTexto(Stream stream)
        {
            DocX doc = DocX.Load(stream);
            IList<Paragraph> Paragrafos = doc.Paragraphs;
            StringBuilder Texto = new StringBuilder();

            foreach (Paragraph p in Paragrafos)
            {
                IEnumerable<Bookmark> Bookmarks = p.GetBookmarks();
                foreach (Bookmark b in Bookmarks)
                {
                    b.Paragraph.InsertAtBookmark(b.Name, b.Name);
                }
                Texto.AppendLine(p.Text);
            }
            return Texto.ToString();
        }

        public string ObterTexto(FileStream stream)
        {
            DocX doc = DocX.Load(stream);
            IList<Paragraph> Paragrafos = doc.Paragraphs;
            StringBuilder Texto = new StringBuilder();
            
            foreach(Paragraph p in Paragrafos)
            {
                IEnumerable<Bookmark> Bookmarks = p.GetBookmarks();
                foreach(Bookmark b in Bookmarks)
                {
                    b.Paragraph.InsertAtBookmark(b.Name, b.Name);
                }
                Texto.AppendLine(p.Text);
            }
            return Texto.ToString();
        }

        public Documento SubstituirCamposDocumento(List<DadosAlunoKV> dadosAluno, Documento documento)
        {
            foreach (DadosAlunoKV dados in dadosAluno)
            {
                if (documento.Texto.Contains(dados.Name.Split(":").Last()))
                {
                    documento.Texto.Replace(dados.Name.Split(":").Last(),dados.Value);
                }
            }
            return documento;
        }

        public Stream SubstituirCamposDocumento(List<DadosAlunoKV> dadosAluno, FileStream stream, int codigo)
        {
            DocX documento = DocX.Load(stream);
            documento.AdicionarCodigoDeGeracao(codigo);
            foreach (DadosAlunoKV dados in dadosAluno)
            {
                if (documento.Bookmarks[dados.Name.Split(":").Last()] != null)
                {
                    documento.Bookmarks[dados.Name.Split(":").Last()].SetText(dados.Value);
                }
            }
            var ms = new MemoryStream();
            documento.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }

    }
}
