using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class InstituicaoViewModel
    {
        public UsuarioADE Usuario { get; set; }
        public PaginatedList<InformacaoInstituicao> PaginaInstituicoes {get; set;}
        public List<Instituicao> Instituicoes {get; set;}
        public bool PrimeiraInstituicao { get; set; }


        public InstituicaoViewModel(UsuarioADE usuario, List<Instituicao> instituicoes, PaginatedList<InformacaoInstituicao> PageInstituicoes, bool primeiraInstituicao)
        {
            Usuario = usuario;
            PaginaInstituicoes = PageInstituicoes;
            Instituicoes = instituicoes;
            PrimeiraInstituicao = primeiraInstituicao;
        }
        public InstituicaoViewModel()
        {}
    }
}
