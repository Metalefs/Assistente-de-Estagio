using ADE.Dominio.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Interfaces;

namespace ADE.Dominio.Models
{
    public partial class Curso : ModeloBase, IRecuperavel, IModeloADE
    {
        public Curso()
        {
            Documento = new HashSet<Documento>();
        }
        [Key]
        [Display(Name = "ID do Curso")]
        public override int Identificador { get; set; }
        [Display(Name = "Nome do Curso")]
        [Required(ErrorMessage = "O campo Nome do Curso é obrigatório")]
        public string NomeCurso { get; set; }
        [Display(Name = "Sigla do Curso")]
        [MaxLength(10,ErrorMessage ="A Sigla deve conter no máximo 10 caractéres")]
        [Required(ErrorMessage = "O campo Sigla do Curso é obrigatório")]
        public string Sigla { get; set; }
        [Display(Name = "Coordenador do Curso")]
        [Required(ErrorMessage = "O campo Coordenador do Curso é obrigatório")]
        public string CoordenadorCurso { get; set; }
        [Display(Name = "E-mail do Coordenador do Curso")]
        public string EmailCoordenadorCurso { get; set; }
        [Display(Name = "Descrição do Curso")]
        [Required(ErrorMessage = "O campo Descrição do Curso é obrigatório")]
        public string DescricaoCurso { get; set; }
        [Display(Name = "Tipo de Curso")]
        [Required(ErrorMessage = "O campo Tipo de Curso é obrigatório")]
        public EnumTipoCurso TipoCurso { get; set; }
        [Display(Name = "Instituição")]
        [Required(ErrorMessage = "O campo Instituição é obrigatório")]
        public int IdInstituicao { get; set; }
        [Display(Name = "Alerta a exibir sobre o curso")]
        public string Alerta { get; set; }
        [Display(Name = "Informação em destaque")]
        public string Informacao { get; set; }
        [Display(Name = "Carga Horária Mínima do Estágio no curso")]
        public int CargaHorariaMinimaEstagio { get; set; }
        
        public override string ToString()
        {
            if (Instituicao == null)
                return NomeCurso;
            if(NomeCurso.Length <= 13 || Sigla == null)
                return string.Format("{0} - {1}", NomeCurso, Instituicao.Nome);
            else
                return string.Format("{0} - {1}", Sigla, Instituicao.Nome);
        }

        public virtual Instituicao Instituicao { get; set; }
        public virtual ICollection<Documento> Documento { get; set; }
        public virtual ICollection<InformacaoCurso> InformacaoCursos { get; set; }
        public virtual RegulamentacaoCurso RegulamentacaoCursos { get; set; }
        public virtual ICollection<AtividadeEstagio> Atividades { get; set; }
        public virtual ICollection<AreaEstagioCurso> AreasEstagio { get; set; }

        public string Nome()
        {
            if (NomeCurso.Length <= 13 || Sigla == null)
                return NomeCurso;
            else
                return Sigla;
        }

        public void Clonar(Curso curso)
        {
            NomeCurso = curso.NomeCurso;
            CoordenadorCurso = curso.CoordenadorCurso;
            EmailCoordenadorCurso = curso.EmailCoordenadorCurso;
            DescricaoCurso = curso.DescricaoCurso;
            TipoCurso = curso.TipoCurso;
            Instituicao = curso.Instituicao;
            Alerta = curso.Alerta;
            Informacao = curso.Informacao;
        }

        public void Recuperar()
        {
            DataHoraExclusao = null;
        }

    }
}
