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
    public class ServicoHistoricoGeracaoDocumento : ServicoBase<HistoricoGeracaoDocumento>, IServicoBase<HistoricoGeracaoDocumento>, ICountable
    {
        private UnitOfWork unitOfWork;
        
        public ServicoHistoricoGeracaoDocumento(
            ref UnitOfWork _unitOfWork
            ) : base (ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override async Task LogAtualizacao(UsuarioADE usuario, HistoricoGeracaoDocumento entity, string Mensagem = null)
        {
            await LogAcao(usuario, entity, "ServicoHistoricoGeracaoDocumento", EnumTipoLog.CriacaoArquivo, TipoEvento.Criacao);
        }

        public override async Task LogCadastramento(UsuarioADE usuario, HistoricoGeracaoDocumento entity)
        {
            await LogAcao(usuario, entity, "ServicoHistoricoGeracaoDocumento", EnumTipoLog.CriacaoArquivo, TipoEvento.Criacao);
        }

        public override async Task LogRemocao(UsuarioADE usuario, HistoricoGeracaoDocumento entity)
        {
            await LogAcao(usuario, entity, "ServicoHistoricoGeracaoDocumento", EnumTipoLog.Outro, TipoEvento.Criacao);
        }

        public async Task<List<HistoricoGeracaoDocumento>> RecuperarHistoricoDoUsuario(string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<HistoricoGeracaoDocumento>().Filtrar(x => x.IdUsuario == IdUsuario);
        }
    }
}
