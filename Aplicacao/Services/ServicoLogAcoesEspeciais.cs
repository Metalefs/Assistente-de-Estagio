using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using ADE.Dominio.Models.Individuais;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADE.Dominio.Models.Enums;
using System.Linq;
using ADE.Infra.Data.UOW;

namespace ADE.Aplicacao.Services
{
    public class ServicoLogAcoesEspeciais : IServicoBase<LogAcoesEspeciais>
    {
        private UnitOfWork unitOfWork;
        public ServicoLogAcoesEspeciais(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task CadastrarAsync(UsuarioADE usuario, LogAcoesEspeciais Log)
        {
            await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Criar(Log);
            Console.WriteLine(Log.DataHoraInclusao + " | " + Log.Mensagem + " | " + Log.LocalOrigem + " | " + Log.AcoesSistema);
            await unitOfWork.Commit();
        }

        public async Task Log(UsuarioADE usuario, string Mensagem, string LocalAcao, EnumTipoLog tipoLog, TipoEvento Acao)
        {
            LogAcoesEspeciais Log = new LogAcoesEspeciais(Mensagem, LocalAcao, tipoLog, usuario.Id);
            await CadastrarAsync(usuario, Log);
        }

        public Task AtualizarAsync(UsuarioADE usuario, LogAcoesEspeciais entity, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(UsuarioADE usuario, LogAcoesEspeciais entity)
        {
            throw new NotImplementedException();
        }

        public async Task<LogAcoesEspeciais> BuscarPorId(int id)
        {
           return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().BuscarUm(x => x.Identificador == id);
        }

        public async Task<List<LogAcoesEspeciais>> Filtrar(Expression<Func<LogAcoesEspeciais, bool>> expression)
        {
            return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Filtrar(expression);
        }

        public async Task<List<LogAcoesEspeciais>> NotificacaoCriacaoAlteracaoDelecaoInstituicao(UsuarioADE usuario)
        {
            return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Filtrar(x => x.AcoesSistema == EnumTipoLog.CriacaoInstituicao && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.AlteracaoInstituicao && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.DelecaoInstituicao&& x.DataHoraInclusao >= usuario.DataHoraInclusao);
        }

        public async Task<List<LogAcoesEspeciais>> NotificacaoCriacaoAlteracaoDelecaoCurso(UsuarioADE usuario)
        {
            return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Filtrar(x => x.AcoesSistema == EnumTipoLog.CriacaoCurso && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.AlteracaoCurso && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.DelecaoCurso&& x.DataHoraInclusao >= usuario.DataHoraInclusao);
        }

        public async Task<List<LogAcoesEspeciais>> NotificacaoCriacaoAlteracaoDelecaoDocumento(UsuarioADE usuario)
        {
            return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Filtrar(x => x.AcoesSistema == EnumTipoLog.CriacaoDocumento && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.AlteracaoDocumento && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.DelecaoDocumento&& x.DataHoraInclusao >= usuario.DataHoraInclusao);
        }

        public async Task<List<LogAcoesEspeciais>> NotificacaoCriacaoAlteracaoDelecaoRequisito(UsuarioADE usuario)
        {
            return await unitOfWork.RepositorioBase<LogAcoesEspeciais>().Filtrar(x => x.AcoesSistema == EnumTipoLog.CriacaoRequisito && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.AlteracaoRequisito && x.DataHoraInclusao >= usuario.DataHoraInclusao|| x.AcoesSistema == EnumTipoLog.DelecaoRequisito&& x.DataHoraInclusao >= usuario.DataHoraInclusao);
        }

        public async Task<List<LogAcoesEspeciais>> ListarAsync()
        {
            List<LogAcoesEspeciais> Logs = await unitOfWork.RepositorioBase<LogAcoesEspeciais>().ListarAsync();
            return Logs.OrderByDescending(x => x.DataHoraInclusao).ToList();
        }
    }
}
