using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoRelacionamento
    {
        [Display(Name = "Amigo")]
        Amigo = 0,
        [Display(Name = "Professor")]
        Professor = 0,
        [Display(Name = "Orientador")]
        Orientador = 0,
        [Display(Name = "Aluno")]
        Aluno = 0
    }
}
