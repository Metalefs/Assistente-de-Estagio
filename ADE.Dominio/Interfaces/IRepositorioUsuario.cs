using System.Threading.Tasks;
using ADE.Dominio.Models.Individuais;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ADE.Dominio.Interfaces
{
    public interface IRepositorioUsuario
    {
        Task<List<UsuarioADE>> ListarUsuarios();
        Task<List<UsuarioADE>> Filtrar(Expression<Func<UsuarioADE, bool>> expression);
        Task<List<UsuarioADE>> TakeBetween(int start, int finish);
        Task<int> CountByInstituicao(int idInstituicao);
        Task<int> CountByCurso(int IdCurso);
        Task<int> Count();
        Task<int> CountLoggedIn();
    }
}
