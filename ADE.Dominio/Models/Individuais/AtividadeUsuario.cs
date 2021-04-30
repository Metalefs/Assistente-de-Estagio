using ADE.Dominio.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class AtividadeUsuario : ModeloBase
    {
        public AtividadeUsuario(string idUsuario, string titulo, string descricao, int idCurso, EnumTipoAtividadeEstagio tipoAtividade)
        {
            IdUsuario = idUsuario;
            Titulo = titulo;
            Descricao = descricao;
            IdCurso = idCurso;
            TipoAtividade = tipoAtividade;
        }

        public AtividadeUsuario()
        {
            Aberto();
        }

        [Key]
        public override int Identificador { get; set; }
        public string IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public EnumVisibilidadeAtividadeUsuario Visibilidade { get; set; }
        public int IdCurso { get; set; }
        public EnumTipoAtividadeEstagio TipoAtividade { get; set; }

        private EnumEstadoAtividade Estado { get; set; }

        public bool Concluido { get; set; }

        public virtual UsuarioADE Usuario { get; set; }

        public void Clonar(AtividadeUsuario atividade)
        {
            this.Titulo = atividade.Titulo;
            this.Descricao = atividade.Descricao;
            this.Data = atividade.Data;
            this.TipoAtividade = atividade.TipoAtividade;
        }

        public override string ToString()
        {
            return Titulo;
        }

        public EnumEstadoAtividade VerificarEstado()
        {
            if (Data < DateTime.Now && Estado != EnumEstadoAtividade.Concluido)
            {
                Atraso();
            }
            return Estado;
        }

        public void Concluir()
        {
            if (DateTime.Now < Data)
                Adiantado();
            else
                AlternarConclusao();
        }
        private void Aberto()
        {
            Estado = EnumEstadoAtividade.Aberto;
            Concluido = false;
        }
        private void AlternarConclusao()
        {
            Concluido = Concluido == true ? false : true;
            Estado = Estado == EnumEstadoAtividade.Concluido ? EnumEstadoAtividade.Aberto : EnumEstadoAtividade.Concluido;
        }
        private void Adiantado()
        {
            Concluido = true;
            Estado = Estado == EnumEstadoAtividade.Adiantado ? EnumEstadoAtividade.Aberto : EnumEstadoAtividade.Adiantado;
        }
        private void Atraso()
        {
            Estado = EnumEstadoAtividade.Atrasado;
        }
    }
}
