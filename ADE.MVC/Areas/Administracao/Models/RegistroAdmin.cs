using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class RegistroAdmin
    {
        [Required(ErrorMessage = "O campo 'E-mail' é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo 'Função' é obrigatório")]
        public EnumTipoUsuario Funcao { get; set; }
        [Required(ErrorMessage = "O campo 'Existente' é obrigatório")]
        public bool Existente { get; set; }
    }
}
