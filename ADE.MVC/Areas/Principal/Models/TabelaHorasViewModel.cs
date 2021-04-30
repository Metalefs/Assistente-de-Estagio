using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class TabelaHorasViewModel
    {
        public UsuarioADE Usuario { get; set; }
        public List<RegistroDeHoras> Registros { get; set; }
        public TabelaHorasViewModel(UsuarioADE usuario, List<RegistroDeHoras> registros)
        {
            Usuario = usuario;
            this.Registros = registros;
        }
        public TabelaHorasViewModel()
        {}
    }
}
