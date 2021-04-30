using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Dominio.Interfaces;
using Assistente_de_Estagio.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ADE.Dominio.Models;
using System;
using System.Linq.Expressions;

namespace ADE.Infra.Data.Repository
{
    public class RepositorioRecuperacaoEntidades<T> : IRepositorioRecuperacaoEntidades<T> where T : ModeloBase, IRecuperavel
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> DbSet;

        public RepositorioRecuperacaoEntidades(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public void Recuperar(T entity)
        {
            entity.Recuperar();
            DbSet.Update(entity);
        }

        public async Task<List<T>> Filtrar(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return new List<T>();
            }
        }

        public async Task<List<T>> ListarAsync()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return new List<T>();
            }
        }

        public async Task<bool> Any()
        {
            try
            {
                return await DbSet.AnyAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public async Task<T> BuscarUm(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await DbSet.Where(expression).SingleAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Count()
        {
            return await DbSet.CountAsync();
        }

        public async Task<int> CountByFilter(Expression<Func<T, bool>> expression)
        {
            return await DbSet.Where(expression).CountAsync();
        }
    }
}
