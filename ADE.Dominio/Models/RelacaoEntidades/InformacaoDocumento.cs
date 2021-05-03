using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class InformacaoDocumento : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public override int Identificador { get; set; }
        public EnumTipoInformacao TipoInformacao { get; set; }
        public int IdDocumento { get; set; }
        public string ConteudoInformacao { get; set; }

        public InformacaoDocumento(EnumTipoInformacao _TipoInformacao, int _IdDocumento, string _ConteudoInformacao)
        {
            this.TipoInformacao = _TipoInformacao;
            this.IdDocumento = _IdDocumento;
            this.ConteudoInformacao = _ConteudoInformacao;
        }

        public InformacaoDocumento(){}

        public override string ToString()
        {
            return string.Format("{0}", TipoInformacao.ToString());
        }

        public virtual Documento IdDocumentoNavigation { get; set; }
    }
}