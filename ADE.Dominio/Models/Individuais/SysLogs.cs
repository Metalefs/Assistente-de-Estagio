using ADE.Dominio.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ADE.Dominio.Models
{
    public partial class SysLogs : ModeloBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string Mensagem { get; set; }
        public string LocalOrigem { get; set; }
        public EnumTipoLog AcoesSistema { get; set; }

        public SysLogs(){}

        public SysLogs(string mensagem, string localOrigem, EnumTipoLog acaoSistema)
        {
            Mensagem = mensagem;
            LocalOrigem = localOrigem;
            AcoesSistema = acaoSistema;
            DataHoraUltimaAlteracao = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Syslog {Identificador} - {Mensagem} - na ação de {Enum.GetName(typeof(EnumTipoLog), AcoesSistema)}";
        }
    }
}
