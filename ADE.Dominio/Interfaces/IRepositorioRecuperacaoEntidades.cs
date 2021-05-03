using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADE.Dominio.Models;

namespace ADE.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoEntidades<T> where T : IRecuperavel
    {
        void Recuperar(T entity);
        Task<List<T>> Filtrar(Expression<Func<T, bool>> expression);
        Task<List<T>> ListarAsync();
        Task<bool> Any();
        Task<T> BuscarUm(Expression<Func<T, bool>> expression);
        Task<int> Count();
        Task<int> CountByFilter(Expression<Func<T, bool>> expression);
    }
}
