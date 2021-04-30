using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumFormatoDocumento
    {
        [Description("docx")]
        docx = 1,
        [Description("pdf")]
        pdf = 2
    }
}
