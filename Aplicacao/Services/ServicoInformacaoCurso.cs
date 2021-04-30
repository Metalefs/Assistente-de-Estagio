using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoInformacaoCurso : ServicoBase<InformacaoCurso>, IServicoBase<InformacaoCurso>, IServicoInformacao<InformacaoCurso>
    {
        public ServicoInformacaoCurso(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork) { }

        public async override Task LogCadastramento(UsuarioADE usuario, InformacaoCurso Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Criacao);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, InformacaoCurso Informacao, string Mensagem = null)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, InformacaoCurso Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoCurso", EnumTipoLog.AlteracaoCurso, TipoEvento.Delecao);
        }

        public async Task<List<InformacaoCurso>> RecuperarInformacao(int IdCurso)
        {
            return await Filtrar(x => x.IdCurso == IdCurso);
        }

    }
}
