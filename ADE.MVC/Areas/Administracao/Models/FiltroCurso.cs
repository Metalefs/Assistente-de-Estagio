using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class FiltroCurso
    {
        public EnumTipoCurso enumTipo { get; set; }
        public string NomeOrientador { get; set; }
        public string NomeFaculdade { get; set; }
    }
}
