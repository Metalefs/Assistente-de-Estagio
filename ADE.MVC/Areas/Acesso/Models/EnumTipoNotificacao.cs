using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public enum EnumTipoNotificacao
    {
        [Description ("Geral")]
        Geral,
        [Description ("Individual")]
        Individual,
        [Description ("Ambos")]
        Ambos,
    }
}
