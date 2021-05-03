using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using System.Linq.Expressions;
using ADE.Dominio.Models.Enums;
using ADE.Infra.Data.UOW;

namespace ADE.Aplicacao.Services
{
    public class ServicoLogins : ServicoBase<Logins>, IServicoBase<Logins>
    {
        private UnitOfWork unitOfWork;

        public ServicoLogins(ref UnitOfWork _unitOfWork) : base (ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async override Task LogCadastramento(UsuarioADE usuario, Logins login)
        {
            string Mensagem = $"{usuario.ToString()} realizou LOGIN no sistema";
            await LogAcao(usuario, Mensagem, "ServicoLogins", EnumTipoLog.Login, TipoEvento.Criacao);
        }

        public async Task DeslogarAsync(UsuarioADE usuario, Logins login)
        {
            unitOfWork.RepositorioBase<Logins>().Editar(login);
            string Mensagem = $"{usuario.ToString()} realizou LOGOUT";
            await LogAcao(usuario, Mensagem, "ServicoLogins", EnumTipoLog.Outro, TipoEvento.Alteracao);
        }

        public async override Task LogAtualizacao(UsuarioADE usuario, Logins login, string Mensagem = null)
        {
            await LogAcao(usuario, login, "ServicoLogins", EnumTipoLog.Outro, TipoEvento.Alteracao);
        }

        public async override Task LogRemocao(UsuarioADE usuario, Logins login)
        {
           await LogAcao(usuario, login, "ServicoLogins", EnumTipoLog.Outro, TipoEvento.Delecao);
        }

        public Logins BuscarUltimoLoginUsuario(string userId)
        {
            return unitOfWork.RepositorioBase<Logins>().Filtrar(x => x.IdUsuario == userId).Result.FindLast(x => x.IdUsuario == userId);
        }
    }
}
