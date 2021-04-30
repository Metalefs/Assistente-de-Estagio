using ADE.Dominio.Models;

namespace Assistente_de_Estagio.Views.Shared.Components.RequisitoInput
{
    public class RequisitoInputViewModel
    {
        public Requisito Requisito { get; set; }
        public string Onchange { get; set; }

        public RequisitoInputViewModel(Requisito requisito, string onchange)
        {
            Requisito = requisito;
            Onchange = onchange;
        }
    }
}
