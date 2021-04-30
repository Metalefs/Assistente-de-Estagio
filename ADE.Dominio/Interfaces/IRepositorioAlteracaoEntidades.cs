using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADE.Dominio.Models;

namespace ADE.Dominio.Interfaces
{
    public interface IRepositorioAlteracaoEntidades
    {
        Task<bool> Any();
        Task<AlteracaoEntidadesSistema> BuscarUm(Expression<Func<AlteracaoEntidadesSistema, bool>> expression);
        Task<int> Count();
        Task<int> CountByFilter(Expression<Func<AlteracaoEntidadesSistema, bool>> expression);
        Task Criar(AlteracaoEntidadesSistema entity);
        void Editar(AlteracaoEntidadesSistema entity);
        Task<List<AlteracaoEntidadesSistema>> Filtrar(Expression<Func<AlteracaoEntidadesSistema, bool>> expression);
        Task<List<AlteracaoEntidadesSistema>> ListarAsync();
        void Remover(AlteracaoEntidadesSistema entity);
    }
}
