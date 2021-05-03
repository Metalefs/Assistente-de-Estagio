using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public enum FiltroPerfil
    {
        [Display(Name = "Instituição")]
        Instituicao,
        [Display(Name = "Curso")]
        Curso,
        [Display(Name = "Pontuação")]
        Pontuacao
    }
}
