using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Dominio.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models.Individuais
{
    public class UsuarioADE : IdentityUser
    {
        public UsuarioADE(ICollection<RequisitoDeUsuario> requisitoDeUsuario, ICollection<HistoricoGeracaoDocumento> historicoGeracaoDocumento)
        {
            RequisitoDeUsuario = requisitoDeUsuario;
            HistoricoGeracaoDocumento = historicoGeracaoDocumento;
        }

        public UsuarioADE() { }
        [PersonalDataAttribute]
        public virtual int IdCurso { get; set; }
        [PersonalDataAttribute]
        public virtual int IdInstituicao { get; set; }
        [PersonalDataAttribute]
        public virtual bool Logado { get; set; }
        public virtual bool Estagiando { get; set; }
        public virtual bool AceitouTermos { get; set; }
        public virtual int Pontuacao { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
        public DateTime? DataHoraExclusao { get; set; }
        public EnumTipoRecebimentoNotificacao TipoRecebimentoNotificacao { get; set; }
        [NotMapped]
        public IFormFile LogoFile { get; set; }
        public byte[] Logo { get; set; }

        public override string ToString()
        {
            if(UserName != Email)
            return string.Format("({0}) - {1}",Email, UserName);
            return string.Format("{0}", UserName);
        }

        public bool PossuiCurso()
        {
            return IdCurso > 0;
        }

        public bool PossuiInstituicao()
        {
            return IdInstituicao > 0;
        }

        public bool ReceberNotificacaoGeral()
        {
            return TipoRecebimentoNotificacao == EnumTipoRecebimentoNotificacao.Geral;
        }

        public bool ReceberNotificacaoFocado()
        {
            return TipoRecebimentoNotificacao == EnumTipoRecebimentoNotificacao.Focado;
        }

        public virtual Curso IdCursoNavigation { get; set; }
        public virtual Instituicao IdInstituicaoNavigation { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<Logins> Logins { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<FAQ> FAQS { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<MensagemIndividual> Mensagems { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<NotificacaoIndividual> NotificacoesIndividuais { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<HistoricoGeracaoDocumento> HistoricoGeracaoDocumento { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<RequisitoDeUsuario> RequisitoDeUsuario { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<RegistroDeHoras> RegistroDeHoras { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<LogAcoesEspeciais> LogAcoesEspeciais { get; set; }
        public virtual ICollection<VisualizacaoNotificacaoGeral> NotificacoesVisualizadas { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<AlteracaoEntidadesSistema> AlteracaoEntidadesSistema { get; set; }
        [PersonalDataAttribute]
        public virtual ICollection<AtividadeUsuario> AtividadesUsuario { get; set; }
        public virtual ICollection<ConclusaoAtividadeCurso> ConclusaoAtividadeCurso { get; set; }
        public virtual ICollection<ListaAmigos> ListaAmigos { get; set; }
    }
}
