using System;
using System.Collections.Generic;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumEmailTemplate
    {
        [Description("ConfirmacaoRegistroADE")]
        Registro = 1,
        [Description("ConfirmacaoRegistroAdministradorADE")]
        RegistroAdministrador = 2,
        [Description("ResetSenhaADE")]
        TrocaSenha = 3,
        [Description("text/csv")]
        DelecaoConta = 4
    }
}
