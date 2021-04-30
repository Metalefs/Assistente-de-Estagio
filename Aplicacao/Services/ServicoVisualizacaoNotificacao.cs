using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.RelacaoEntidades;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoVisualizacaoNotificacaoGeral : ServicoBase<VisualizacaoNotificacaoGeral>
    {
        private UnitOfWork unitOfWork;

        public ServicoVisualizacaoNotificacaoGeral(ref UnitOfWork _unitOfWork) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override Task LogCadastramento(UsuarioADE usuario, VisualizacaoNotificacaoGeral entidade)
        {
            throw new NotImplementedException();
        }

        public override Task LogAtualizacao(UsuarioADE usuario, VisualizacaoNotificacaoGeral entidade, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        public override Task LogRemocao(UsuarioADE usuario, VisualizacaoNotificacaoGeral entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VisualizacaoNotificacaoGeral>> BuscarPorIdUsuario(string IdUsuario)
        {
            return await unitOfWork.RepositorioBase<VisualizacaoNotificacaoGeral>().Filtrar(x => x.IdUsuario == IdUsuario);
        }
        
    }
}
