using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models
{
    public partial class AreaEstagioCurso : ModeloBase
    {
        public AreaEstagioCurso(int idCurso, string nome)
        {
            IdCurso = idCurso;
            Nome = nome;
        }

        public AreaEstagioCurso()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public override int Identificador { get; set; }
        public int IdCurso { get; set; }
        public string Nome { get; set; }

        public virtual Curso IdCursoNavigation {get;set;}
    }
}
