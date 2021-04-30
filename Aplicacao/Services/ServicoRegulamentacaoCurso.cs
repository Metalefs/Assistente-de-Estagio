using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoRegulamentacaoCurso : ServicoBase<RegulamentacaoCurso>, IServicoBase<RegulamentacaoCurso>
    {
        public ServicoRegulamentacaoCurso(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork) { }

        public async override Task LogCadastramento(UsuarioADE usuario, RegulamentacaoCurso Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoRegulamentacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Criacao);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, RegulamentacaoCurso Informacao, string Mensagem = null)
        {
            await LogAcao(usuario, Informacao, "ServicoRegulamentacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, RegulamentacaoCurso Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoRegulamentacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Delecao);
        }

        public async Task<RegulamentacaoCurso> RecuperarRegulamentacao(int IdCurso)
        {
            return await BuscarUm(x => x.IdCurso == IdCurso);
        }

    }
}
