using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class AlteracoesAdministradorViewModel
    {
       public UsuarioADE Usuario { get; set; }
       public PaginatedList<AlteracaoEntidadesSistema> Alteracoes { get; set; }
    }
}
