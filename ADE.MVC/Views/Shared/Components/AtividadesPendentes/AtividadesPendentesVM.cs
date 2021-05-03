
using ADE.Dominio.Models.Individuais;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Views.RegistroHoras.Components.AtividadesPendentes
{
    public class AtividadesPendentesVM
    {
        public List<AtividadeUsuario> Registros { get; set; }
        public List<AtividadeEstagio> AtividadesEstagio { get; set; }
        public Logins UltimoLogin { get; set; }
        public AtividadesPendentesVM(List<AtividadeUsuario> registros, List<AtividadeEstagio> atividadesEstagio, Logins ultimoLogin)
        {
            Registros = registros;
            AtividadesEstagio = atividadesEstagio;
            UltimoLogin = ultimoLogin;
        }
    }
}
