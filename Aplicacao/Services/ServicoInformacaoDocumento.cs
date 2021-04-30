using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoInformacaoDocumento : ServicoBase<InformacaoDocumento>, IServicoBase<InformacaoDocumento>, IServicoInformacao<InformacaoDocumento>
    {
        public ServicoInformacaoDocumento(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork) { }

        public async override Task LogCadastramento(UsuarioADE usuario, InformacaoDocumento Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoDocumento", EnumTipoLog.AlteracaoDocumento, TipoEvento.Criacao); 
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, InformacaoDocumento Informacao, string Mensagem = null)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoDocumento", EnumTipoLog.AlteracaoDocumento, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, InformacaoDocumento Informacao)
        {
            await LogAcao(usuario, Informacao, "ServicoInformacaoDocumento", EnumTipoLog.AlteracaoDocumento, TipoEvento.Delecao);
        }

        public async Task<List<InformacaoDocumento>> RecuperarInformacao(int IdDocumento)
        {
            return await Filtrar(x => x.IdDocumento == IdDocumento);
        }
    }
}
