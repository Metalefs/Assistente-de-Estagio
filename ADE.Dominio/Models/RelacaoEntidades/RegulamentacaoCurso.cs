using ADE.Dominio.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class RegulamentacaoCurso : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public override int Identificador { get; set; }
        public int IdCurso { get; set; }
        public string Endereco { get; set; }

        public RegulamentacaoCurso(int _IdCurso, string Endereco)
        {
            this.IdCurso = _IdCurso;
            this.Endereco = Endereco;
        }

        public RegulamentacaoCurso(){}

        public virtual Curso IdCursoNavigation { get; set; }
    }
}