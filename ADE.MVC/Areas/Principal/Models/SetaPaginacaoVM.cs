using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class SetaPaginacaoVM<T>
    {
        public PaginatedList<T> Paginated { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
    }
}
