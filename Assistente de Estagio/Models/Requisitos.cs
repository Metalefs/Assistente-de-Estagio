using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Requisitos
    {
        public int IdRequisito { get; set; }
        public string NomeRequisito { get; set; }
        public string Dados { get; set; }
        public string Descricao { get; set; }
        public string Opcoes { get; set; }
        public string Tipo { get; set; }
        public string Tag { get; set; }
    }
}
