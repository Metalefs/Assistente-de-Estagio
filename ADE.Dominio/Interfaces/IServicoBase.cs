using ADE.Dominio.Models.Individuais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ADE.Dominio.Interfaces
{
    public interface IServicoBase<T>
    {
        Task CadastrarAsync(UsuarioADE usuario,T entity);
        Task AtualizarAsync(UsuarioADE usuario, T entity, string Mensagem = null);
        Task RemoverAsync(UsuarioADE usuario, T entity);
        Task<List<T>> ListarAsync();
        Task<List<T>> Filtrar(Expression<Func<T, bool>> expression);
        Task<T> BuscarPorId(int id);
    }
}
