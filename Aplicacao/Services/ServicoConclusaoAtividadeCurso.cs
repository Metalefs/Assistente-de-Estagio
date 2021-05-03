using ADE.Dominio.Models;
using ADE.Infra.Data.UOW;
using ADE.Dominio.Models.RelacaoEntidades;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Interfaces;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using ADE.Utilidades.Extensions;

namespace ADE.Aplicacao.Services
{
    public class ServicoConclusaoAtividadeCurso: ICountable
    {
        private UnitOfWork unitOfWork;
        private ServicoNotificacaoIndividual ServicoNotificacaoIndividual;
        public ServicoConclusaoAtividadeCurso(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ServicoNotificacaoIndividual = new ServicoNotificacaoIndividual(ref unitOfWork);
        }

        public async Task CadastrarAsync(UsuarioADE usuario, ConclusaoAtividadeCurso entity, AtividadeEstagio atividade)
        {
            if( await CountByFilter(x=>x.IdAtividade == entity.IdAtividade && x.IdUsuario == entity.IdUsuario) == 0)
            {
              entity.Identificador = await ObterId();
              await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Criar(entity);
              await CriarNotificacaoConclusaoAtividade(usuario, atividade);
              await unitOfWork.Commit();
            }
        }
        public async Task CriarNotificacaoConclusaoAtividade(UsuarioADE usuario, AtividadeEstagio atividade)
        {
            ServicoAlteracaoEntidadesSistema servicoAlteracaoEntidadesSistema = new ServicoAlteracaoEntidadesSistema(ref unitOfWork);
            ModeloBase recurso = await servicoAlteracaoEntidadesSistema.ObterEntidadeAlteracao(atividade.EnumEntidade, atividade.IdentificadorEntidadeAtividade);
            string Descricao = $"<i class='material-icons medium prefix'>assignment_turned_in</i> Você concluiu a atividade {atividade.Titulo} ao realizar {atividade.TipoAtividade.ObterNomeEnum()} do recurso {recurso.ToString()} na data {atividade.DataHoraUltimaAlteracao.ToLocalTime()}";
            NotificacaoIndividual notificacao = new NotificacaoIndividual(usuario.Id, usuario.Id, $"Atividade {atividade.Titulo} Concluida!", Descricao);
            await ServicoNotificacaoIndividual.CadastrarAsync(usuario, notificacao);
            await unitOfWork.Commit();
        }

        public async Task<int> ObterId()
        {
            return await Count() + 1;
        }

        public async Task AtualizarAsync(UsuarioADE usuario, ConclusaoAtividadeCurso entity)
        {
            unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Editar(entity);
            await unitOfWork.Commit();
        }

        public async Task<ConclusaoAtividadeCurso> BuscarUm(Expression<Func<ConclusaoAtividadeCurso, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().BuscarUm(expression);
        }

        public async Task RemoverAsync(UsuarioADE usuario, ConclusaoAtividadeCurso entity)
        {
            unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Remover(entity);
            await unitOfWork.Commit();
        }

        public async Task<List<ConclusaoAtividadeCurso>> ListarAsync()
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().ListarAsync();
        }

        public async Task<List<ConclusaoAtividadeCurso>> Filtrar(Expression<Func<ConclusaoAtividadeCurso, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Filtrar(expression);
        }

        public async Task<List<ConclusaoAtividadeCurso>> FiltrarComQuantidade(Expression<Func<ConclusaoAtividadeCurso, bool>> expression, int quantidade)
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().FiltrarComQuantidade(expression, quantidade);
        }

        public async Task<ConclusaoAtividadeCurso> BuscarPorId(int id)
        {
            try
            {
                return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().BuscarUm(x => x.Identificador == id);
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public async Task<int> Count()
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Count();
        }
        public async Task<int> CountByFilter(Expression<Func<ConclusaoAtividadeCurso, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().CountByFilter(expression);
        }

        public async Task AtualizarAsync(UsuarioADE usuario, ConclusaoAtividadeCurso entity, string Mensagem = null)
        {
            unitOfWork.RepositorioBase<ConclusaoAtividadeCurso>().Editar(entity);
        }
    }
}
