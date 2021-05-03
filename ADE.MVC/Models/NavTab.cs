namespace ADE.Apresentacao.Models.NavTab
{
    public class NavTab
    {
        public ItemPrincipal ItemPrincipal { get; set; }
        public LoadType LoadType { get; set; }
        public AreaComponente AreaComponente { get; set; }
        public NavItem[] NavItems { get; set; }
    }

    public class ItemPrincipal
    {
        public NavItem NavItem { get; set; }
    }

}
