using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoOpcaoRequisito : ServicoBase<OpcaoRequisito>, IServicoBase<OpcaoRequisito>
    {
        private UnitOfWork unitOfWork;

        public ServicoOpcaoRequisito(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, OpcaoRequisito entity, string Mensagem = null)
        {
            await LogAcao(usuario, entity, "ServicoOpcaoRequisito", EnumTipoLog.AlteracaoRequisito, TipoEvento.Alteracao);
        }

        public async override Task LogCadastramento(UsuarioADE usuario, OpcaoRequisito entity)
        {
            await LogAcao(usuario, entity, "ServicoOpcaoRequisito", EnumTipoLog.CriacaoRequisito, TipoEvento.Criacao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, OpcaoRequisito entity)
        {
            await LogAcao(usuario, entity, "ServicoOpcaoRequisito", EnumTipoLog.DelecaoRequisito, TipoEvento.Delecao);
        }

        public async Task<OpcaoRequisito> BuscarPorId(int idRequisito, string Valor)
        {
            return await unitOfWork.RepositorioBase<OpcaoRequisito>().BuscarUm(x => x.IdRequisito == idRequisito && x.Valor == Valor);
        }

        public async Task<List<OpcaoRequisito>> ListarPorRequisito(int idRequisito)
        {
            return await unitOfWork.RepositorioBase<OpcaoRequisito>().Filtrar(x => x.IdRequisito == idRequisito);
        }
    }
}
