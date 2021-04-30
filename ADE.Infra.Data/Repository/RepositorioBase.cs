using Assistente_de_Estagio.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using ADE.Dominio.Models;
using ADE.Dominio.Interfaces;

namespace ADE.Infra.Data.Repository
{
    public partial class RepositorioBase<T> : IRepositorioBase<T> where T : ModeloBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> DbSet;

        public RepositorioBase(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public async Task Criar(T entity)
        {
            entity.DataHoraInclusao = DateTime.Now;
            await DbSet.AddAsync(entity);
        }

        public void Editar(T entity)
        {
            entity.DataHoraUltimaAlteracao = DateTime.Now;
            DbSet.Update(entity);
        }

        public void Remover(T entity)
        {
            entity.DataHoraExclusao = DateTime.Now;
            Editar(entity);
        }

        public async Task<List<T>> Filtrar(Expression<Func<T, bool>> expression)
        {
            try
            {
                var List = DbSet.Where(expression).AsEnumerable();
                return List.Where(x => x.DataHoraExclusao == null).ToList();
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
                return await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return new List<T>();
            }
        }

        public async Task<T> BuscarUm(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).Where(expression).SingleOrDefaultAsync();
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

        public async Task<T> BuscarPorId(int id)
        {
            try
            {
                return await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).Where(x=> x.Identificador == id).SingleOrDefaultAsync();
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

        public async Task<int> Count() => await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).CountAsync();

        public async Task<int> CountByFilter(Expression<Func<T, bool>> expression) => await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).Where(expression).CountAsync();


        public async Task<List<T>> FiltrarComQuantidade(Expression<Func<T, bool>> expression, int quantidade)
        {
            try
            {
                return await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).Where(expression).Take(quantidade).ToListAsync();
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

        public async Task<T> FiltrarUltimo(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await DbSet.Where(x => x.DataHoraExclusao == null && x.Identificador > 0).Where(expression).LastOrDefaultAsync();
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
    }
}
