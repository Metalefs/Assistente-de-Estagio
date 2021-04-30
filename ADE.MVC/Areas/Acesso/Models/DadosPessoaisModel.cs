using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public class DadosPessoaisModel
    {
        [Required]
        public string Password { get; set; }
    }
}
