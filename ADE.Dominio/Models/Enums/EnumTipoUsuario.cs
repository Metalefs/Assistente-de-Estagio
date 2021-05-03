using System;
using System.Collections.Generic;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoUsuario
    {
        [Description("Admin")]
        Admin,
        [Description("Membro")]
        Membro,
        [Description("CriadorConteudo")]
        CriadorConteudo,
    }
}
