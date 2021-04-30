using ADE.Dominio.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models { 

    public abstract partial class ModeloBase : IModeloADE
    {
        [Key]
        public abstract int Identificador { get; set; }
        public DateTime DataHoraInclusao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
        public DateTime? DataHoraExclusao { get; set; }

        public string EntidadeExcluida()
        {
            return "Esta entidade pode ter side excluida";
        }
    }
}
