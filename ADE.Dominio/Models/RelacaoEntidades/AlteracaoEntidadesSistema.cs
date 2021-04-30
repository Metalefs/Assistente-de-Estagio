using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models
{
    public partial class AlteracaoEntidadesSistema : ModeloBase
    {
        [Key]
        public override int Identificador { get; set; }
        public virtual UsuarioADE Autor  { get; set; }
        public virtual string IdAutor  { get; set; }
        public virtual EnumEntidadesSistema Entidade { get; set; }
        public virtual int IdentificadorEntidade { get; set; }
        public virtual string MensagemAlteracao  { get; set; }

        public virtual ICollection<VisualizacaoNotificacaoGeral> Notificacao { get; set; }
    }
}
