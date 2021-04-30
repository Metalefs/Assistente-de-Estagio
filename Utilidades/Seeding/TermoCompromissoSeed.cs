using ADE.Dominio.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace ADE.Utilidades.Seeding
{
    public static class TermoCompromissoSeed
    {
        public static async Task<TermoCompromisso> TermoInicial(IHostingEnvironment env)
        {
            string TermoPath;
            if (env.IsDevelopment())
                TermoPath = Path.Combine(env.WebRootPath.Replace("bin\\Debug\\netcoreapp2.2\\", ""), "TermosDeCompromisso.txt");
            
            else
                TermoPath = Path.Combine(env.WebRootPath, "TermosDeCompromisso.txt");
            
            return new TermoCompromisso("Termos", await File.ReadAllTextAsync(TermoPath), "1.1");
        }
    }
}
