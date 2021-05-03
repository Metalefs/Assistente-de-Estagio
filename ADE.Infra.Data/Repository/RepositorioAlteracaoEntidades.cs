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
    public class RepositorioAlteracaoEntidades : IRepositorioAlteracaoEntidades
    {
        protected ApplicationDbContext _context;

        public RepositorioAlteracaoEntidades(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Any()
        {
            return await _context.AlteracaoEntidadesSistema.AnyAsync();
        }

        public async Task<AlteracaoEntidadesSistema> BuscarUm(Expression<Func<AlteracaoEntidadesSistema, bool>> expression)
        {
            return await _context.AlteracaoEntidadesSistema.Where(expression).Take(1).FirstAsync();
        }

        public async Task<int> Count()
        {
            return await _context.AlteracaoEntidadesSistema.CountAsync();
        }

        public async Task<int> CountByFilter(Expression<Func<AlteracaoEntidadesSistema, bool>> expression)
        {
            return await _context.AlteracaoEntidadesSistema.Where(expression).CountAsync();
        }

        public async Task Criar(AlteracaoEntidadesSistema entity)
        {
            entity.DataHoraInclusao = DateTime.Now;
            await _context.AlteracaoEntidadesSistema.AddAsync(entity);
        }

        public void Editar(AlteracaoEntidadesSistema entity)
        {
            entity.DataHoraUltimaAlteracao = DateTime.Now;
            _context.AlteracaoEntidadesSistema.Update(entity);
        }

        public async Task<List<AlteracaoEntidadesSistema>> Filtrar(Expression<Func<AlteracaoEntidadesSistema, bool>> expression)
        {
            return await _context.AlteracaoEntidadesSistema.Where(expression).ToListAsync();
        }

        public async Task<List<AlteracaoEntidadesSistema>> ListarAsync()
        {
            return await _context.AlteracaoEntidadesSistema.ToListAsync();
        }

        public void Remover(AlteracaoEntidadesSistema entity)
        {
            throw new NotImplementedException();
        }
    }
}
