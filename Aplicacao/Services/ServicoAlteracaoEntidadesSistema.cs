using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoAlteracaoEntidadesSistema : IServicoAlteracaoEntidadesSistema
    {
        private UnitOfWork unitOfWork;
        private ServicoLogAcoesEspeciais logAcoesEspeciais;
        public ServicoAlteracaoEntidadesSistema(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            logAcoesEspeciais = new ServicoLogAcoesEspeciais(ref unitOfWork);
        }

        public void Atualizar(AlteracaoEntidadesSistema entity)
        {
            unitOfWork.RepositorioAlteracaoEntidades.Editar(entity);
        }

        public async Task<AlteracaoEntidadesSistema> BuscarPorId(int id)
        {
            return await unitOfWork.RepositorioAlteracaoEntidades.BuscarUm(x=>x.Identificador == id);
        }

        public async Task CadastrarAsync(AlteracaoEntidadesSistema entity, EnumTipoLog TipoLog)
        {
            await unitOfWork.RepositorioAlteracaoEntidades.Criar(entity);
            await logAcoesEspeciais.Log(entity.Autor, entity.MensagemAlteracao,"ServicoAlteracaoEntidadesSistema", TipoLog, TipoEvento.Criacao);
            Console.WriteLine(entity.MensagemAlteracao);
        }

        public async Task<List<AlteracaoEntidadesSistema>> Filtrar(Expression<Func<AlteracaoEntidadesSistema, bool>> expression)
        {
            List<AlteracaoEntidadesSistema> Alteracoes = await unitOfWork.RepositorioAlteracaoEntidades.Filtrar(expression);
            return Alteracoes.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

        public async Task<List<AlteracaoEntidadesSistema>> FiltrarNotificacoes(UsuarioADE usuario, List<VisualizacaoNotificacaoGeral> NotificacoesVisualizadas)
        {
            List<AlteracaoEntidadesSistema> Notificacoes = await unitOfWork.RepositorioAlteracaoEntidades.Filtrar(x => x.DataHoraInclusao >= usuario.DataHoraInclusao && x.Entidade != EnumEntidadesSistema.FAQ);
            List<AlteracaoEntidadesSistema> NotificacoesNovas = new List<AlteracaoEntidadesSistema>();    
            foreach(AlteracaoEntidadesSistema notificacao in Notificacoes)
            {
                bool Compativel = NotificacoesVisualizadas.Any(x => x.IdNotificacao == notificacao.Identificador);
                if (NotificacoesVisualizadas.Count == 0 || !Compativel)
                {
                    NotificacoesNovas.Add(notificacao);
                }
            }
            NotificacoesNovas = NotificacoesNovas.OrderByDescending(x => x.DataHoraInclusao).ToList();
            return NotificacoesNovas;
        }
        public async Task<ModeloBase> ObterEntidadeAlteracao(EnumEntidadesSistema TipoEntidadesSistema, int IdEntidade)
        {
            try
            {
                switch (TipoEntidadesSistema)
                {
                    case EnumEntidadesSistema.Instituicao:
                        return await unitOfWork.RepositorioBase<Instituicao>().BuscarPorId(IdEntidade);
                    case EnumEntidadesSistema.Curso:
                        return await unitOfWork.RepositorioBase<Curso>().BuscarPorId(IdEntidade);
                    case EnumEntidadesSistema.Documento:
                        return await unitOfWork.RepositorioBase<Documento>().BuscarPorId(IdEntidade);
                    case EnumEntidadesSistema.Requisito:
                        return await unitOfWork.RepositorioBase<Requisito>().BuscarPorId(IdEntidade);
                    case EnumEntidadesSistema.FAQ:
                        return await unitOfWork.RepositorioBase<FAQ>().BuscarPorId(IdEntidade);
                    case EnumEntidadesSistema.Notificacao:
                        return await unitOfWork.RepositorioBase<NotificacaoIndividual>().BuscarPorId(IdEntidade);
                }
                return new NotificacaoIndividual() { Cabecalho = "Essa entidade pode ter sido excluida" };
            }
            catch (Exception)
            {
                return new NotificacaoIndividual() { Cabecalho = "Essa entidade pode ter sido excluida"};
            }
        }
        public async Task<List<AlteracaoEntidadesSistema>> ListarAsync()
        {
            return await unitOfWork.RepositorioAlteracaoEntidades.ListarAsync();
        }
        public void RemoverAsync(AlteracaoEntidadesSistema entity)
        {
            unitOfWork.RepositorioAlteracaoEntidades.Remover(entity);
        }
    }
}
