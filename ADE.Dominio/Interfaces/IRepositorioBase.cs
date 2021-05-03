using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces
{
    public interface IRepositorioBase <T>
    {
        Task Criar(T entity);
        void Editar(T entity);
        Task<List<T>> Filtrar(Expression<Func<T, bool>> expression);
        Task<List<T>> FiltrarComQuantidade(Expression<Func<T, bool>> expression, int quantidade);
        Task<T> FiltrarUltimo(Expression<Func<T, bool>> expression);
        Task<List<T>> ListarAsync();
        void Remover(T entity);
        Task<bool> Any();
        Task<T> BuscarUm(Expression<Func<T, bool>> expression);
        Task<T> BuscarPorId(int id);
        Task<int> Count();
        Task<int> CountByFilter(Expression<Func<T, bool>> expression);
    }
}
