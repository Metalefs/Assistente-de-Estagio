using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Apresentacao.Models
{
    public enum AreaComponente
    {
        [Description("Default")]
        Usuario,
        [Description("Admin")]
        Admin,
        [Description("Mobile")]
        Mobile
    }
}
