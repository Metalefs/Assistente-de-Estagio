using Assistente_de_Estagio.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Services
{
    public class UsuarioService
    {

        public readonly u2019_estgContext _context;
        private IHostingEnvironment _env;


        public UsuarioService(u2019_estgContext context, IHostingEnvironment env)
        {
            _env = env;
            _context = context;
        }

       public Prioridade CheckUsuario(string NomeEmail, string senha)
        {
            try
            {
                Usuario usuario = _context.Usuario.Where(x => x.EmailUsuario == NomeEmail && x.SenhaUsuario == senha || (x.NomeUsuario == NomeEmail && x.SenhaUsuario == senha)).FirstOrDefault();
                return (Prioridade)Enum.Parse(typeof(Prioridade), usuario.Prioridade, true);
            }
            catch
            {
                return Prioridade.Admin;
            }
            
        }

    }
}
