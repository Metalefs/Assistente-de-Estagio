
using ADE.Dominio.Models;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.UltimosRegistros
{
    public class UltimosRegistrosVM
    {
        public List<RegistroDeHoras> Registros { get; set; }
        public int TotalRegistros { get; set; }

        public UltimosRegistrosVM(List<RegistroDeHoras> registros, int totalRegistros)
        {
            Registros = registros;
            TotalRegistros = totalRegistros;
        }
    }
}
