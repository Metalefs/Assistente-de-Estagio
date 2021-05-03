using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Infra.Data.UOW;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Aplicacao.Services
{
    public class ServicoRecuperacaoEntidades<T> where T : ModeloBase, IRecuperavel
    {
        private UnitOfWork unitOfWork;
        private IRepositorioRecuperacaoEntidades<T> RepositorioRecuperacao;

        public ServicoRecuperacaoEntidades(ref UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            RepositorioRecuperacao = unitOfWork.RepositorioRecuperacao<T>();
        }

        public async Task<T> BuscarPorId(int id)
        {
            return await RepositorioRecuperacao.BuscarUm(x=>x.Identificador == id);
        }

        public async Task<List<T>> Filtrar(Expression<Func<T, bool>> expression)
        {
            return await RepositorioRecuperacao.Filtrar(expression);
        }

        public async Task<List<T>> ListarAsync()
        {
            return await RepositorioRecuperacao.ListarAsync();
        }
    }
}
