using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using ADE.Utilidades.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoRequisito : ServicoBase<Requisito>, IServicoBase<Requisito>, ICountable
    {
        private UnitOfWork unitOfWork;
        private ServicoOpcaoRequisito ServicoOpcaoRequisito;

        public ServicoRequisito(ref UnitOfWork _unitOfWork) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ServicoOpcaoRequisito = new ServicoOpcaoRequisito(ref unitOfWork);
        }

        public async Task<Requisito> BuscarUm(UsuarioADE usuario, Expression<Func<Requisito, bool>> expression)
        {
            try
            {
                return await unitOfWork.RepositorioBase<Requisito>().BuscarUm(expression);
            }
            catch (Exception ex)
            {
                await LogError(usuario, ex.Message, ex.Source, EnumTipoLog.AlteracaoRequisito);
                throw new Exception("Não foi possivel encontrar um resultado para a expressão");
            }
        }

        public new async Task<List<Requisito>> ListarAsync()
        {
            List<Requisito> lista = await base.ListarAsync();
            foreach (Requisito req in lista)
            {
                req.OpcaoRequisito = await ServicoOpcaoRequisito.ListarPorRequisito(req.Identificador);
            }
            return lista;
        }   

        public async override Task LogCadastramento (UsuarioADE usuario, Requisito requisito)
        {
            await LogAcao(usuario, requisito, "ServicoRequisito", EnumTipoLog.CriacaoRequisito, TipoEvento.Criacao);
            await LogAlteracaoEntidade(usuario, new Requisito(), requisito, EnumEntidadesSistema.Requisito, EnumTipoLog.CriacaoRequisito);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, Requisito requisito, string Mensagem = null)
        {
            Requisito RequisitoAntigo = await BuscarPorId(requisito.Identificador);
            await LogAlteracaoEntidade(usuario, RequisitoAntigo, requisito, EnumEntidadesSistema.Requisito, EnumTipoLog.AlteracaoRequisito, Mensagem);
            await LogAcao(usuario, requisito, "ServicoRequisito", EnumTipoLog.AlteracaoRequisito, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, Requisito requisito)
        {
            await LogAcao(usuario, requisito, "ServicoRequisito", EnumTipoLog.DelecaoRequisito, TipoEvento.Delecao);
            await LogDelecaoEntidade(usuario, requisito, EnumEntidadesSistema.Requisito, EnumTipoLog.DelecaoRequisito);
        }

        public new async Task AtualizarAsync(UsuarioADE usuario, Requisito requisito)
        {
            List<Requisito> lista = RequisitoSeed.RequisitosBase();
            if (!lista.Any(x => x.Bookmark == requisito.Bookmark))
            {
                unitOfWork.RepositorioBase<Requisito>().Editar(requisito);
            }

        }
        public new async Task RemoverAsync(UsuarioADE usuario, Requisito requisito)
        {
            List<Requisito> lista = RequisitoSeed.RequisitosBase();
            if(! lista.Any(x=> x.Bookmark == requisito.Bookmark))
            {
                unitOfWork.RepositorioBase<Requisito>().Remover(requisito);
            }
        }

    }
}
