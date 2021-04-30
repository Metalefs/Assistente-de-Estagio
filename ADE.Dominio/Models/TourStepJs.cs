
namespace ADE.Apresentacao.Models
{
    public class TourStepJs
    {
        public string element {get;set;}
        public string title {get;set;}
        public string content { get; set; }

        public TourStepJs(string element, string title, string content)
        {
            this.element = element;
            this.title = title;
            this.content = content;
        }
    }
}
