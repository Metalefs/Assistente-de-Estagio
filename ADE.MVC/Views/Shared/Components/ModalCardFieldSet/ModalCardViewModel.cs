using ADE.Apresentacao.Models;
using Microsoft.AspNetCore.Html;

namespace Assistente_de_Estagio.Areas.Shared
{
    public class ModalCardViewModel
    {
        public string ModalId { get; set; }
        public string Title { get; set; }
        public IHtmlContent Content { get; set; }
        public string Texto { get; set; }
        public string Color { get; set; }
        public bool CanClose { get; set; }
        public bool DefaultOpen { get; set; }
        public string Action { get; set; }
        public TipoModal Tipo { get; set; }

        public ModalCardViewModel(string modalId, string title, IHtmlContent content, TipoModal tipo, string color = null, string action = null, string texto = null)
        {
            ModalId = modalId;
            Title = title;
            Content = content;
            Color = color;
            Action = action;
            Tipo = tipo;
            Texto = texto;
        }

        public ModalCardViewModel(string modalId, string title, string color = null, string action = null, string texto = null)
        {
            ModalId = modalId;
            Title = title;
            Color = color;
            Action = action;
            Texto = texto;
        }

    }
}
