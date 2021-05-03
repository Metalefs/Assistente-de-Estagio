
using ADE.Utilidades.Constants;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADE.Utilidades.Handlers
{
    public static class TextParser
    {
        public static HtmlString MontarMensagemHTML(this IHtmlHelper helper, string Mensagem, string classe = null)
        {
            Mensagem = Mensagem.Replace(HTMLPlaceholders.Div(false), HTMLElements.Div(false)).Replace(HTMLPlaceholders.Div(true), HTMLElements.Div(true));
            if(classe != null)
            {
                Mensagem = Mensagem.Replace(HTMLPlaceholders.Label(false), HTMLElements.Label(false, classe)).Replace(HTMLPlaceholders.Label(true), HTMLElements.Label(true));
                Mensagem = Mensagem.Replace(HTMLPlaceholders.P(false), HTMLElements.P(false, classe)).Replace(HTMLPlaceholders.P(true), HTMLElements.P(true));
            }
            else
            {
                Mensagem = Mensagem.Replace(HTMLPlaceholders.Label(false), HTMLElements.Label(false, "label h6-responsive")).Replace(HTMLPlaceholders.Label(true), HTMLElements.Label(true));
                Mensagem = Mensagem.Replace(HTMLPlaceholders.P(false), HTMLElements.P(false, "alteracao-p")).Replace(HTMLPlaceholders.P(true), HTMLElements.P(true));
            }
            HtmlString html = new HtmlString(Mensagem);
            return html;
        }

        public static string MontarMensagemHTML(string Mensagem, string classe = null)
        {
            Mensagem = Mensagem.Replace(HTMLPlaceholders.Div(false), HTMLElements.Div(false)).Replace(HTMLPlaceholders.Div(true), HTMLElements.Div(true));
            if (classe != null)
            {
                Mensagem = Mensagem.Replace(HTMLPlaceholders.Label(false), HTMLElements.Label(false, classe)).Replace(HTMLPlaceholders.Label(true), HTMLElements.Label(true));
                Mensagem = Mensagem.Replace(HTMLPlaceholders.P(false), HTMLElements.P(false, classe)).Replace(HTMLPlaceholders.P(true), HTMLElements.P(true));
            }
            else
            {
                Mensagem = Mensagem.Replace(HTMLPlaceholders.Label(false), HTMLElements.Label(false, "label h6-responsive")).Replace(HTMLPlaceholders.Label(true), HTMLElements.Label(true));
                Mensagem = Mensagem.Replace(HTMLPlaceholders.P(false), HTMLElements.P(false, "alteracao-p")).Replace(HTMLPlaceholders.P(true), HTMLElements.P(true));
            }
            return Mensagem;
        }

        public static string AninharEmElemento(string elemento, string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, $"<{elemento} {propriedades}>");
            conteudo = conteudo.Insert(conteudo.Length, $"</{elemento}>");
            return conteudo;
        }

        public static string AninharEmPlaceholderDeDiv(string conteudo)
        {
            conteudo = conteudo.Insert(0, HTMLPlaceholders.Div(false));
            conteudo = conteudo.Insert(conteudo.Length, HTMLPlaceholders.Div(true));
            return conteudo;
        }

        public static string AninharEmDiv(string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, HTMLElements.Div(false, propriedades));
            conteudo = conteudo.Insert(conteudo.Length, HTMLElements.Div(true));
            return conteudo;
        }

        public static string AninharEmDiv(ref string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, HTMLElements.Div(false, propriedades));
            conteudo = conteudo.Insert(conteudo.Length, HTMLElements.Div(true));
            return conteudo;
        }

        public static string AninharEmForm(string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, HTMLElements.Form(false, propriedades));
            conteudo = conteudo.Insert(conteudo.Length, HTMLElements.Form(true));
            return conteudo;
        }

        public static string AninharEmPlaceholderDeParagrafo(string conteudo)
        {
            conteudo = conteudo.Insert(0, HTMLPlaceholders.P(false));
            conteudo = conteudo.Insert(conteudo.Length, HTMLPlaceholders.P(true));
            return conteudo;
        }

        public static string AninharEmParagrafo(string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, HTMLElements.P(false, propriedades));
            conteudo = conteudo.Insert(conteudo.Length, HTMLElements.P(true));
            return conteudo;
        }

        public static string AninharEmPlaceholderDeLabel(string conteudo)
        {
            conteudo = conteudo.Insert(0, HTMLPlaceholders.Label(false));
            conteudo = conteudo.Insert(conteudo.Length, HTMLPlaceholders.Label(true));
            return conteudo;
        }

        public static string AninharEmLabel(string conteudo, string propriedades = null)
        {
            conteudo = conteudo.Insert(0, HTMLElements.Label(false, propriedades));
            conteudo = conteudo.Insert(conteudo.Length, HTMLElements.Label(true));
            return conteudo;
        }
    }
}
