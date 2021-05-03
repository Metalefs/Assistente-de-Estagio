using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ADE.Utilidades.Handlers.TextParser;
using static ADE.Utilidades.Helpers.PropertyGenerator;

namespace ADE.Utilidades.Extensions
{
    public static class HTMLExtensions
    {
        public static HtmlString CreatePageHeader(this IHtmlHelper helper, string HeaderText)
        {
            string Propriedades = CreateHTMLProperty("class", "hoverable card-header Page-Title text-muted modal-header h4-responsive cursor-pointer") + CreateHTMLProperty("onclick", ""); ;
            string Elemento = AninharEmElemento("span", HeaderText, Propriedades);
            HtmlString html = new HtmlString(AninharEmDiv(Elemento, CreateHTMLProperty("class", "Page-Title")));
            return html;
        }

        public static HtmlString CreatePageHeaderWithBackArrow(this IHtmlHelper helper, string HeaderText)
        {
            string anchorPropriedades = CreateHTMLProperty("class", "fa fa-angle-left float-lg-right") + CreateHTMLProperty("onclick", "window.history.back();") + CreateHTMLProperty("title", "Voltar");
            string DivPropriedades = CreateHTMLProperty("class", "hoverable card-header Page-Title text-muted modal-header h4-responsive cursor-pointer") + CreateHTMLProperty("onclick", "");
            string Div = AninharEmElemento("div", HeaderText, DivPropriedades);
            string anchor = AninharEmElemento("a", "", anchorPropriedades);
            HtmlString html = new HtmlString(AninharEmDiv(Div + anchor, CreateHTMLProperty("class","Page-Title")));
            return html;
        }

        public static HtmlString CreateBackArrow(this IHtmlHelper helper, string HeaderText)
        {
            string anchorPropriedades = CreateHTMLProperty("class", "fa fa-angle-left float-lg-right") + CreateHTMLProperty("onclick", "window.history.back();");
            string anchor = AninharEmElemento("a", "", anchorPropriedades);
            HtmlString html = new HtmlString(AninharEmDiv(anchor));
            return html;
        }

        public static HtmlString CreatePageHeader(string HeaderText)
        {
            string Propriedades = CreateHTMLProperty("class", "card-header Page-Title modal-header h4-responsive");
            string Elemento = AninharEmElemento("span", HeaderText, Propriedades);
            HtmlString html = new HtmlString(Elemento);
            return html;
        }

    }
}
