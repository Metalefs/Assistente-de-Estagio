using ADE.Dominio.Models;
namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class InformacaoCursoVM
    {
        public Curso Curso { get; set; }
        public int QuantidadeAlunosCurso;
        public int QuantidadeDocumentosCurso;
        public bool CursoDoUsuario { get; set; }

        public InformacaoCursoVM(Curso curso, int quantidadeAlunosCurso, int quantidadeDocumentosCurso, bool cursoDoUsuario = false)
        {
            Curso = curso;
            QuantidadeAlunosCurso = quantidadeAlunosCurso;
            QuantidadeDocumentosCurso = quantidadeDocumentosCurso;
            CursoDoUsuario = cursoDoUsuario;
        }
        public InformacaoCursoVM()
        {}
    }
}
