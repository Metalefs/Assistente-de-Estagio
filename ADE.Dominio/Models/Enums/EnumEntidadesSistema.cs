using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Enums
{
    public enum EnumEntidadesSistema
    {
        [Display(Name = "Curso")]
        Curso = 1,
        [Display(Name = "Documento")]
        Documento = 2,
        [Display(Name = "HistoricoGeracaoDocumento")]
        HistoricoGeracaoDocumento = 3,
        [Display(Name = "LogAcoesEspeciais")]
        LogAcoesEspeciais = 4,
        [Display(Name = "Logins")]
        Logins = 5,
        [Display(Name = "Requisito")]
        Requisito = 6,
        [Display(Name = "Syslogs")]
        Syslogs = 7,
        [Display(Name = "OpcaoRequisito")]
        OpcaoRequisito = 8,
        [Display(Name = "RequisitoDeDocumento")]
        RequisitoDeDocumento = 9,
        [Display(Name = "RequisitoDeUsuario")]
        RequisitoDeUsuario = 10,
        [Display(Name = "Instituicao")]
        Instituicao = 11,
        [Display(Name = "FAQ")]
        FAQ = 12,
        [Display(Name = "Notificacao")]
        Notificacao = 13,
        [Display(Name = "RegistroHoras")]
        RegistroHoras = 14,
        [Display(Name = "TermoCompromisso")]
        TermoCompromisso = 14
    }
}
