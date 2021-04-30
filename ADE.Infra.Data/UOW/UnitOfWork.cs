using ADE.Dominio.Interfaces;
using ADE.Dominio.Models;
using ADE.Infra.Data.Repository;
using Assistente_de_Estagio.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ADE.Infra.Data.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext _context;
        private IRepositorioAlteracaoEntidades _repositorioAlteracaoEntidades;
        private IRepositorioUsuario _repositorioUsuario;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositorioBase<T> RepositorioBase<T>() where T : ModeloBase
        {
            return new RepositorioBase<T>(_context);
        }
        public IRepositorioAlteracaoEntidades RepositorioAlteracaoEntidades
        {
            get { return _repositorioAlteracaoEntidades ?? (_repositorioAlteracaoEntidades = new RepositorioAlteracaoEntidades(_context)); }
        }

        public IRepositorioRecuperacaoEntidades<T> RepositorioRecuperacao<T>() where T : ModeloBase, IRecuperavel
        {
            return new RepositorioRecuperacaoEntidades<T>(_context);
        }
        public IRepositorioUsuario RepositorioUsuario
        {
            get { return _repositorioUsuario ?? (_repositorioUsuario = new RepositorioUsuario(_context)); }
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                Console.WriteLine("\n" + ex);
            }
        }

        public void Attach<T>(T entity) where T: ModeloBase
        {
            _context.Attach<T>(entity);
        }

        public void SetStateUnchanged<T>(T entity) where T: ModeloBase
        {
            _context.Entry(entity).State = EntityState.Unchanged;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
