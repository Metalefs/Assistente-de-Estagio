using System.ComponentModel;using System.ComponentModel.DataAnnotations;
namespace ADE.Dominio.Models.Enums
{
    public enum EnumEtapaDocumento
    {
        [Display(Name = "Inicial")]
        Inicial = 1,
        [Display(Name = "Intermediaria ")]
        Intermediaria = 2,
        [Display(Name = "Final")]
        Final = 3
    }
}
