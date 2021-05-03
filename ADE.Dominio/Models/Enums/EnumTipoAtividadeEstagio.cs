using System.ComponentModel;using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumTipoAtividadeEstagio
    {
        [Display(Name = "Completar fluxo")]
        CompletarFluxo = 0,
        [Display(Name = "Exportação")]
        Exportacao = 1,
        [Display(Name = "Download ou Impressão")]
        DownloadOuImpressao = 2,
        [Display(Name = "Download")]
        Download = 3,
        [Display(Name = "Impressão")]
        Impressao = 4,
        [Display(Name = "Registrar Hora")]
        RegistrarHora = 5,
        [Display(Name = "Atualizar Dados")]
        AtualizarDados = 6,
        [Display(Name = "Completar Carga-horária")]
        CompletarCargaHoraria = 7
    }
}
