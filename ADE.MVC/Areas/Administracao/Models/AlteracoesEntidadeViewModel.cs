using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class AlteracoesEntidadeViewModel
    {
        public string DescricaoEntidade { get; set; }
        public List<AlteracaoEntidadesSistema> Alteracoes { get; set; }
    }
}
