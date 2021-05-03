using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class UsuariosAdministradoresViewmodel
    {
        public PaginatedList<UsuarioADE> Admins { get; set; }
        public PaginatedList<UsuarioADE> CriadoresConteudo { get; set; }
    }
}
