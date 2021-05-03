namespace ADE.Apresentacao.Models
{

    public class Elemento
    {
        public string Nome { get; set; }
        public AreaComponente AreaComponente { get; set; }
        public string Icone { get; set; }
        public string Link { get; set; }
        public string IdCollapsible { get; set; }
        public NavItem[] NavItems { get; set; }
    }
}
