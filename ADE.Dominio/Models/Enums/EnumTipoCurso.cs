using System;
using System.Collections.Generic;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoCurso
    {
        [Display(Name = "Bacharel")]
        Bacharel = 1,
        [Display(Name = "Graduação")]
        Graduacao = 2,
        [Display(Name = "Tecnólogo")]
        Tecnologo = 3,
        [Display(Name = "Técnico")]
        Tecnico = 4,
        [Display(Name = "Ensino Médio")]
        EnsinoMedio = 5
    }
}
