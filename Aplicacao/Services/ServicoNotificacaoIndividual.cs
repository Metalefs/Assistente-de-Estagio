using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoNotificacaoIndividual: IServicoBase<NotificacaoIndividual>
    {
        private UnitOfWork unitOfWork;

        public ServicoNotificacaoIndividual(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        
        public async Task<List<NotificacaoIndividual>> BuscarPorIdUsuario(string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<NotificacaoIndividual>().Filtrar(x => x.IdUsuarioRemetente == IdUsuario && x.Status != EnumStatusNotificacaoIndividual.Lido || x.IdUsuarioDestino == IdUsuario && x.Status != EnumStatusNotificacaoIndividual.Lido);
        }

        public async Task<List<NotificacaoIndividual>> BuscarHistoricoPorIdUsuario(string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<NotificacaoIndividual>().Filtrar(x => x.IdUsuarioRemetente == IdUsuario  || x.IdUsuarioDestino == IdUsuario );
        }

        public async Task CadastrarAsync(UsuarioADE usuario, NotificacaoIndividual entidade)
        {
            await unitOfWork.RepositorioBase<NotificacaoIndividual>().Criar(entidade);
            await unitOfWork.Commit();
        }

        public async Task Dismiss(UsuarioADE usuario, NotificacaoIndividual entity)
        {
            entity.Status = EnumStatusNotificacaoIndividual.Lido;
            await AtualizarAsync(usuario, entity);
        }

        public async Task Dismiss(UsuarioADE usuario, int IdEntity)
        {
            NotificacaoIndividual entity = await BuscarPorId(IdEntity);
            entity.Status = EnumStatusNotificacaoIndividual.Lido;
            await AtualizarAsync(usuario, entity);
        }

        public async Task AtualizarAsync(UsuarioADE usuario, NotificacaoIndividual entity, string Mensagem = null)
        {
            unitOfWork.RepositorioBase<NotificacaoIndividual>().Editar(entity);
            await unitOfWork.Commit();
        }

        public Task RemoverAsync(UsuarioADE usuario, NotificacaoIndividual entity)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificacaoIndividual> BuscarPorId(int id)
        {
            return await unitOfWork.RepositorioBase<NotificacaoIndividual>().BuscarUm(x => x.Identificador == id);
        }

        public async Task<List<NotificacaoIndividual>> Filtrar(Expression<Func<NotificacaoIndividual, bool>> expression)
        {
            List<NotificacaoIndividual> Lista = await unitOfWork.RepositorioBase<NotificacaoIndividual>().Filtrar(expression);
            return Lista.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

        public async Task<List<NotificacaoIndividual>> ListarAsync()
        {
            List<NotificacaoIndividual> Logs = await unitOfWork.RepositorioBase<NotificacaoIndividual>().ListarAsync();
            return Logs.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

    }
}
