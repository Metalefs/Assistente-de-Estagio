
using ADE.Dominio.Models.Individuais;
namespace ADE.Apresentacao.Models.Sidenav
{
    public class Sidebar
    {
        public string Titulo { get; set; }
        public string SrcLogo { get; set; }
        public LoadType LoadType { get; set; }
        public string LinkPrincipal { get; set; }
        public Elemento[] Elementos { get; set; }
        public UsuarioADE UsuarioADE { get; set; }
    }

}
