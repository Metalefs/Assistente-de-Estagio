using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using System.Collections.Generic;

namespace ADE.Aplicacao.Services
{
    public class ServicoAreaEstagioCurso : ServicoBase<AreaEstagioCurso>, IServicoBase<AreaEstagioCurso>, ICountable
    {
        private UnitOfWork unitOfWork;

        public ServicoAreaEstagioCurso(ref UnitOfWork _unitOfWork) : base(ref _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override Task LogCadastramento(UsuarioADE usuario, AreaEstagioCurso Curso)
        {
            throw new NotImplementedException();
        }
        
        public override Task LogAtualizacao(UsuarioADE usuario, AreaEstagioCurso CursoNovo, string Mensagem = null)
        {
            throw new NotImplementedException();
        }

        private Task LogAtualizacaoComTituloAntigo(UsuarioADE usuario, AreaEstagioCurso Curso, string TituloAntigo)
        {
            throw new NotImplementedException();
        }

        public override Task LogRemocao(UsuarioADE usuario, AreaEstagioCurso Curso)
        {
            throw new NotImplementedException();
        }

        public new async Task<AreaEstagioCurso> BuscarPorId(int id)
        {
            try
            {
                return await unitOfWork.RepositorioBase<AreaEstagioCurso>().BuscarUm(x=>x.Identificador == id);
            }
            catch (InvalidOperationException)
            {
                return new AreaEstagioCurso();
            }
        }
        public async Task<List<AreaEstagioCurso>> BuscarPorIdCurso(int id)
        {
            try
            {
                return await unitOfWork.RepositorioBase<AreaEstagioCurso>().Filtrar(x => x.IdCurso == id);
            }
            catch (InvalidOperationException)
            {
                return new List<AreaEstagioCurso>();
            }
        }
    }
}
