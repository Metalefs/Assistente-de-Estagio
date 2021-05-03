using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoDocumento
    {
        [Display(Name = "Regulamento")]
        Regulamento = 1,
        [Display(Name = "Aviso")]
        Aviso = 2,
        [Display(Name = "Preenchimento")]
        Preenchimento = 3,
        [Display(Name = "Tabela De Registro de Horas")]
        Tabela = 4,
        [Display(Name = "Questionario")]
        Questionario = 5,
        [Display(Name = "Explicativo")]
        Explicativo = 6
    }
}
