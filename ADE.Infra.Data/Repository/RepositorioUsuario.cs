using System.Collections.Generic;
using System.Threading.Tasks;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace ADE.Infra.Data.Repository
{
    public class RepositorioUsuario: IRepositorioUsuario
    {
        protected ApplicationDbContext _context;

        public RepositorioUsuario(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioADE>> ListarUsuarios()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<UsuarioADE>> Filtrar(Expression<Func<UsuarioADE, bool>> expression)
        {
            return await _context.Users.Where(expression).ToListAsync();
        }

        public async Task<List<UsuarioADE>> TakeBetween(int start, int finish)
        {
            return await _context.Users.OrderBy(x=>x.Id).Skip(start).Take(finish).ToListAsync();
        }

        public async Task<int> CountByInstituicao(int idInstituicao)
        {
            return await _context.Users.Where(x=> x.IdInstituicao == idInstituicao).CountAsync();
        }

        public async Task<int> CountByCurso(int idCurso)
        {
            return await _context.Users.Where(x => x.IdCurso == idCurso).CountAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Users.CountAsync();
        }
        
        public async Task<int> CountLoggedIn()
        {
            return await _context.Users.Where(x=> x.Logado == true).CountAsync();
        }
    }
}
