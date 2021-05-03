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
    public class ServicoMensagemIndividual: IServicoBase<MensagemIndividual>
    {
        private UnitOfWork unitOfWork;

        public ServicoMensagemIndividual(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        
        public async Task<List<MensagemIndividual>> BuscarConversaPorIdUsuario(string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<MensagemIndividual>().Filtrar(x => x.IdUsuarioRemetente == IdUsuario && x.Status != EnumStatusMensagem.Lido || x.IdUsuarioDestino == IdUsuario && x.Status != EnumStatusMensagem.Lido);
        }

        public async Task<List<MensagemIndividual>> BuscarConversaComUsuario(UsuarioADE usuario, string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<MensagemIndividual>().Filtrar(x => x.IdUsuarioRemetente == usuario.Id || x.IdUsuarioDestino == IdUsuario );
        }

        public async Task CadastrarAsync(UsuarioADE usuario, MensagemIndividual entidade)
        {
            await unitOfWork.RepositorioBase<MensagemIndividual>().Criar(entidade);
            await unitOfWork.Commit();
        }

        public async Task Dismiss(UsuarioADE usuario, MensagemIndividual entity)
        {
            entity.Status = EnumStatusMensagem.Lido;
            await AtualizarAsync(usuario, entity);
        }

        public async Task Dismiss(UsuarioADE usuario, int IdEntity)
        {
            MensagemIndividual entity = await BuscarPorId(IdEntity);
            entity.Status = EnumStatusMensagem.Lido;
            await AtualizarAsync(usuario, entity);
        }

        public async Task AtualizarAsync(UsuarioADE usuario, MensagemIndividual entity, string Mensagem = null)
        {
            unitOfWork.RepositorioBase<MensagemIndividual>().Editar(entity);
            await unitOfWork.Commit();
        }

        public Task RemoverAsync(UsuarioADE usuario, MensagemIndividual entity)
        {
            throw new NotImplementedException();
        }

        public async Task<MensagemIndividual> BuscarPorId(int id)
        {
            return await unitOfWork.RepositorioBase<MensagemIndividual>().BuscarUm(x => x.Identificador == id);
        }

        public async Task<List<MensagemIndividual>> Filtrar(Expression<Func<MensagemIndividual, bool>> expression)
        {
            List<MensagemIndividual> Lista = await unitOfWork.RepositorioBase<MensagemIndividual>().Filtrar(expression);
            return Lista.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

        public async Task<List<MensagemIndividual>> ListarAsync()
        {
            List<MensagemIndividual> Logs = await unitOfWork.RepositorioBase<MensagemIndividual>().ListarAsync();
            return Logs.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }

    }
}
