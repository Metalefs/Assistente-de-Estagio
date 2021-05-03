using ADE.Dominio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class VisualizarEntidadeViewmodel<T> where T: ModeloBase
    {
        public List<AlteracaoEntidadesSistema> ListaAlteracoes { get; set; }
        public T Entidade { get; set; }
    }
}
