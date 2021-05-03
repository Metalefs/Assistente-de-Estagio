using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumEstadoAtividade
    {
        [Description("Aberto")]
        Aberto = 1,
        [Description("Atrasado")]
        Atrasado = 2,
        [Description("Adiantado")]
        Adiantado = 3,
        [Description("Concluido")]
        Concluido = 4
    }
}
