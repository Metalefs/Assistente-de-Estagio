using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class Documento : ModeloBase, IRecuperavel, IModeloADE
    {
        public Documento()
        {
            RequisitoDeDocumento = new HashSet<RequisitoDeDocumento>();
        }
        public Documento(int IdCurso, string TituloDocumento, string DescricaoDocumento, sbyte PosicaoDocumento)
        {
            this.IdCurso = IdCurso;
            this.TituloDocumento = TituloDocumento;
            this.DescricaoDocumento = DescricaoDocumento;
            this.PosicaoDocumento = PosicaoDocumento;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        [Required]
        public int IdCurso { get; set; }

        [NotMapped]
        public IFormFile ArquivoFormulario {get;set;} 
        public byte[] Arquivo { get; set; }
        [NotMapped]
        public IFormFile ArquivoPDFFormulario {get;set;}
        public byte[] ArquivoPDF { get; set; }

        [Required(ErrorMessage = "O Campo Titulo do Documento é obrigatório")]
        [Display(Name = "Titulo do Documento")]
        public string TituloDocumento { get; set; }
        [Display(Name = "Descrição do documento")]
        public string DescricaoDocumento { get; set; }
        [Display(Name = "Posição de exibição")]
        public sbyte PosicaoDocumento { get; set; }
        [Display(Name = "Assinatura")]
        public EnumAssinaturaDocumento Assinatura { get; set; }
        [Display(Name = "Aviso")]
        public string Aviso { get; set; }
        [Required(ErrorMessage = "O Campo Tipo do Documento é obrigatório")]
        [Display(Name = "Tipo de documento")]
        public EnumTipoDocumento Tipo { get; set; }
        [Display(Name = "Texto do documento")]
        public string Texto { get; set; }
        [Required(ErrorMessage = "O Campo Etapa é obrigatório")]
        [Display(Name = "Etapa preferencial de preenchimento")]
        public EnumEtapaDocumento Etapa { get; set; }
        public EnumVisibilidade Visibilidade { get; set; }
        [Display(Name="Exibir campo Assinatura do responsável de estágio na empresa")]
        public bool PossuiAssinaturaResposavelEstagio { get; set; }
        [Display(Name="Exibir campo Carimbo CNPJ")]
        public bool PossuiCarimboCNPJ { get; set; }
        [Display(Name="Exibir campo Data")]
        public bool PossuiData { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - ({1} - {2})", TituloDocumento, Tipo, Assinatura);
        }

        public virtual Curso IdCursoNavigation { get; set; }
        public virtual ICollection<RequisitoDeDocumento> RequisitoDeDocumento { get; set; }
        public virtual ICollection<HistoricoGeracaoDocumento> IdHistoricoGeracaoDocumento { get; set; }
        public virtual ICollection<InformacaoDocumento> InformacaoDocumentos { get; set; }

        public void Clonar(Documento documento)
        {
            IdCurso = documento.IdCurso;
            TituloDocumento = documento.TituloDocumento;
            DescricaoDocumento = documento.DescricaoDocumento;
            PosicaoDocumento = documento.PosicaoDocumento;
            Assinatura = documento.Assinatura;
            Aviso = documento.Aviso;
            Tipo = documento.Tipo;
            Texto = documento.Texto;
            Etapa = documento.Etapa;
            Visibilidade = documento.Visibilidade;
        }
        public void Recuperar()
        {
            DataHoraExclusao = null;
        }

    }
}