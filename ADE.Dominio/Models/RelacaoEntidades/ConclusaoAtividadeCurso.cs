using ADE.Dominio.Models.Individuais;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models.RelacaoEntidades
{
    public class ConclusaoAtividadeCurso : ModeloBase
    {
        public ConclusaoAtividadeCurso(string idUsuario, int idAtividade)
        {
            IdUsuario = idUsuario;
            IdAtividade = idAtividade;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        public string IdUsuario { get; set; }
        public int IdAtividade { get; set; }

        public virtual AtividadeEstagio Atividade { get; set; }
        public virtual UsuarioADE Usuario { get; set; }
    }
}
