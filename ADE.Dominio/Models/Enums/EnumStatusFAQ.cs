using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumStatusFAQ
    {
        [Description("Aberto")]
        Aberto,
        [Description("Respondido")]
        Respondido,
        [Description("Rejeitado")]
        Rejeitado
    }
}
