using ADE.Dominio.Models;
using System.Collections.Generic;
using System;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Shared
{
    public class Notificacao
    {
        public int Identificador { get; set; }
        public string Mensagem { get; set; }
        public string Cabecalho { get; set; }
        public IModeloADE Entidade { get; set; }
        public string TipoEntidade { get; set; }
        public string IdAutor { get; set; }
        public string NomeAutor { get; set; }
        public DateTime Data { get; set; }

        public Notificacao(AlteracaoEntidadesSistema Evento, IModeloADE Entidade)
        {
            Identificador = Evento.Identificador;
            Mensagem = Evento.MensagemAlteracao;
            IdAutor = Evento.IdAutor;
            NomeAutor = Evento.Autor.UserName;
            Data = Evento.DataHoraInclusao;
            this.Entidade = Entidade;
            TipoEntidade = Entidade.GetType().Name;
        }

        public Notificacao(NotificacaoIndividual Evento, IModeloADE Entidade)
        {
            Identificador = Evento.Identificador;
            Mensagem = Evento.Conteudo;
            Mensagem = Evento.Cabecalho;
            IdAutor = Evento.IdUsuarioRemetente;
            Data = Evento.DataHoraInclusao;
            this.Entidade = Entidade;
            TipoEntidade = Entidade.GetType().Name;
        }
    }
}
