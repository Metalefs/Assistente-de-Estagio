using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class AtividadesViewModel
    {
        public UsuarioADE Usuario { get; set; }
        public List<AtividadeEstagio> AtividadesEstagio { get; set; }
        public List<AtividadeUsuario> AtividadeUsuario { get; set; }
        public AtividadesViewModel(UsuarioADE usuario, List<AtividadeEstagio> Atividades, List<AtividadeUsuario> AtividadeUsuario)
        {
            Usuario = usuario;
            this.AtividadesEstagio = Atividades;
            this.AtividadeUsuario = AtividadeUsuario;
        }
        public AtividadesViewModel()
        {}
    }
}
