using ADE.Dominio.Models;
using ADE.GeradorArquivo.Word;
using ADE.Utilidades.Constants;
using Novacode;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADE.GeradorArquivo.Word.Contratos
{
    public class Formulario : ContratoBase
    {
        List<AreaEstagioCurso> Areas;
        int IdArea;
        public Formulario(Documento documento, Cabecalho cabecalho, List<DadosAlunoKV> dados, List<AreaEstagioCurso> areas = null, int idArea = 0) : base(cabecalho,dados,documento)
        {
            Areas = areas;
            IdArea = idArea;
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
                    GerarCorpoDocumento(doc);
                    GerarCampoAdicionais(doc);
                    doc.SaveAs(ms);
                }
            }
            ms.Position = 0;
            return ms;
        }

        public void GerarCorpoDocumento(DocX documento)
        {
            Paragraph section = documento.InsertParagraph(Documento.Texto);
            Formatting formatting = new Formatting();
            Font font = new Font("Arial");
            //formatting.UnderlineStyle = UnderlineStyle.singleLine;
            formatting.FontFamily = font;
            formatting.Size = 12;

            section.Font("Arial");
            section.FontSize(12);
            section.IndentationBefore = 2f;
            foreach (DadosAlunoKV dado in Dados)
            {
                if (dado.Requisito == null) continue;
                string Nome = dado.Requisito.Bookmark;
                string Valor = string.IsNullOrWhiteSpace(dado.Value) ? " ": dado.Value.PadRight(4);
                if (section.Text.Contains(Nome))
                {
                    if(!dado.Requisito.InText)
                        section.ReplaceText(Nome, Valor, false, System.Text.RegularExpressions.RegexOptions.None, formatting);
                    else
                        section.ReplaceText(Nome, Valor);
                }
            }
            if (section.Text.Contains(NomesRequisitos.EscolhaAreaEstagio_))
            {
                if (Dados.Any(x => x.Name.Contains(NomesRequisitos.AreasEstagio_))){
                    string EscolhaArea = Dados.Where(x => x.Name.Contains(NomesRequisitos.AreasEstagio_)).First().Value;
                    section.ReplaceText(NomesRequisitos.EscolhaAreaEstagio_, EscolhaArea);
                }
                else
                {
                    section.ReplaceText(NomesRequisitos.EscolhaAreaEstagio_, "");
                }
            }
            if (section.Text.Contains(NomesRequisitos.CursoAluno_))
            {
                if (Dados.Any(x => x.Name.Contains(NomesRequisitos.CursoAluno_))){
                    string EscolhaArea = Dados.Where(x => x.Name.Contains(NomesRequisitos.CursoAluno_)).First().Value;
                    section.ReplaceText(NomesRequisitos.CursoAluno_, EscolhaArea);
                }
                else
                {
                    section.ReplaceText(NomesRequisitos.CursoAluno_, "");
                }
            }
            if(Areas != null)
            {
                if (section.Text.Contains(NomesRequisitos.TabelaAreasEstagio_))
                    section.ReplaceText(NomesRequisitos.TabelaAreasEstagio_, "");

                Formatting Format = new Formatting();
                Format.Bold = true;

                documento.InsertParagraph("ÁREAS DE ESTÁGIO OFERECIDAS", false, Format).FontSize(14).Alignment = Alignment.center;
                documento.InsertParagraph("").AppendLine();
                documento.GerarTabelaAreaDeEstagio(Areas, IdArea);
            }
        }

        public void GerarCampoAdicionais(DocX doc)
        {
            doc.InsertParagraph(System.Environment.NewLine).AppendLine();
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
