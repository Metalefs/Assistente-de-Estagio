using Assistente_de_Estagio.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public class PerfisViewModel
    {
        public PaginatedList<UsuarioADE> Usuarios { get; set; }
        public List<ListaAmigos> Amigos { get; set; }
        public int Paginas { get; set; }
        public int PaginaAtual { get; set; }


        public PerfisViewModel() { }

        public PerfisViewModel(PaginatedList<UsuarioADE> usuarios)
        {
            Usuarios = usuarios;
        }
    }
}
