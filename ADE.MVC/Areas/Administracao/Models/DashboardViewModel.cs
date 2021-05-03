using System.Collections.Generic;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using System.Linq;
using Assistente_de_Estagio.Areas.Shared;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class DashboardViewModel
    {
        public int TotalDownloads { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalUsuariosLogados { get; set; }

        public int TotalCursos { get; set; }
        public int TotalDocumentos { get; set; }
        public int TotalRequisitos { get; set; }

        public PaginatedList<LogAcoesEspeciais> LogAcoes { get; set; }
        public PaginatedList<SysLogs> ErrorLogs { get; set; }

        public DashboardViewModel()
        {
        }
    }
}
