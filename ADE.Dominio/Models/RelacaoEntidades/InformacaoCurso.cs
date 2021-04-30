using ADE.Dominio.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class InformacaoCurso : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public override int Identificador { get; set; }
        public EnumTipoInformacao TipoInformacao { get; set; }
        public int IdCurso { get; set; }
        public string ConteudoInformacao { get; set; }

        public InformacaoCurso(EnumTipoInformacao _TipoInformacao, int _IdCurso, string _ConteudoInformacao)
        {
            this.TipoInformacao = _TipoInformacao;
            this.IdCurso = _IdCurso;
            this.ConteudoInformacao = _ConteudoInformacao;
        }

        public InformacaoCurso(){}

        public override string ToString()
        {
            return string.Format("{0}", TipoInformacao.ToString());
        }

        public virtual Curso IdCursoNavigation { get; set; }
    }
}