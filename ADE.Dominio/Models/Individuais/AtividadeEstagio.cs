using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Dominio.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class AtividadeEstagio : ModeloBase, IRecuperavel
    {
        public AtividadeEstagio(string titulo, string descricao, int idCurso, EnumTipoAtividadeEstagio tipoAtividade, int condicaoVezes, EnumEntidadesSistema enumEntidade, int identificadorEntidadeAtividade)
        {
            Titulo = titulo;
            Descricao = descricao;
            IdCurso = idCurso;
            TipoAtividade = tipoAtividade;
            CondicaoVezes = condicaoVezes;
            EnumEntidade = enumEntidade;
            IdentificadorEntidadeAtividade = identificadorEntidadeAtividade;
        }

        public AtividadeEstagio()
        {
            ConclusoesAtividade = new List<ConclusaoAtividadeCurso>();
        }

        public void Recuperar()
        {
            DataHoraExclusao = null;
        }

        [Key]
        public override int Identificador { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int IdCurso { get; set; }
        public EnumTipoAtividadeEstagio TipoAtividade { get; set; }
        public int CondicaoVezes { get; set; }
        public EnumEntidadesSistema EnumEntidade { get; set; }
        public int IdentificadorEntidadeAtividade { get; set; }
        public virtual List<ConclusaoAtividadeCurso> ConclusoesAtividade { get; set; }

        public virtual Curso Curso { get; set; }
    }
}
