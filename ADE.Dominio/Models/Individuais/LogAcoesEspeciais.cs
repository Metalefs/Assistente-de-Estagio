using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.RelacaoEntidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models.Individuais
{
    public partial class LogAcoesEspeciais : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string Mensagem { get; set; }
        public string IdUsuario { get; set; }
        public string LocalOrigem { get; set; }
        public EnumTipoLog AcoesSistema { get; set; }

        public LogAcoesEspeciais(){}

        public LogAcoesEspeciais(string mensagem, string localOrigem, EnumTipoLog acaoSistema, string idUsuario)
        {
            Mensagem = mensagem;
            LocalOrigem = localOrigem;
            AcoesSistema = acaoSistema;
            IdUsuario = idUsuario;
            DataHoraUltimaAlteracao = DateTime.Now;
        }
        
        public virtual UsuarioADE IdUsuarioNavigation { get; set; }
    }
}
