using System.ComponentModel.DataAnnotations;
namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoRecebimentoNotificacao
    {
        [Display(Name = "Geral")]
        Geral = 1,
        [Display(Name = "Focado")]
        Focado = 2,
        [Display(Name = "Desabilitado")]
        Desabilitado = 3 
    }
}
