using ADE.Dominio.Models.Individuais;
using System.Collections.Generic;
using ADE.Dominio.Models;
using ADE.Dominio.Models.RelacaoEntidades;

namespace Assistente_de_Estagio.Models
{
    public class PerfilUsuarioViewModel
    {
        public int QuantidadeDocumentosGerados { get; set; }
        public IList<string> AutorizacaoUsuario { get; set; }
        public UsuarioADE Usuario { get; set; }
        public Curso CursoUsuario { get; set; }

        public List<ListaAmigos> ListaAmigos { get; set; }


        public PerfilUsuarioViewModel() { }

        public PerfilUsuarioViewModel(int quantidadeDocumentosGerados, IList<string> autorizacaoUsuario, UsuarioADE usuario, Curso cursoUsuario)
        {
            QuantidadeDocumentosGerados = quantidadeDocumentosGerados;
            AutorizacaoUsuario = autorizacaoUsuario;
            Usuario = usuario;
            CursoUsuario = cursoUsuario;
        }
    }
}
