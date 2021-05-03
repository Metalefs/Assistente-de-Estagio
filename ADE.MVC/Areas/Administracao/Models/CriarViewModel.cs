using ADE.Dominio.Models;

namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class CriarViewModel
    {
        public CriarDocumentoViewModel CriarDocumentoViewModel { get; set; }
        public Curso Curso { get; set; }
        public Requisito Requisito { get; set; }
        public string Selection { get; set; }

        public CriarViewModel(string selection, CriarDocumentoViewModel criarDocumentoViewModel, Curso curso, Requisito requisito)
        {
            CriarDocumentoViewModel = criarDocumentoViewModel;
            Curso = curso;
            Requisito = requisito;
            Selection = selection;
        }
    }
}

