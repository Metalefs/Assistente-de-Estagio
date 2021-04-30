using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Enums;

namespace ADE.Dominio.Models
{
    public partial class Requisito : ModeloBase, IRecuperavel, IModeloADE
    {
        public Requisito()
        {
            OpcaoRequisito = new HashSet<OpcaoRequisito>();
            RequisitoDeUsuario = new HashSet<RequisitoDeUsuario>();
        }

        public Requisito(string nomeRequisito, string descricao, EnumElementoHTMLRequisito ElementoHTMLRequisito, EnumTipoRequisito tipoRequisito, string Bookmark, EnumGrupoRequisito Grupo, int size, bool inText, string MascaraEntrada = null, bool Obrigatorio = false, List <OpcaoRequisito> Opcoes = null)
        {
            this.NomeRequisito = nomeRequisito;
            this.Descricao = descricao;
            this.ElementoHTMLRequisito = ElementoHTMLRequisito;
            this.TipoElementoHTMLRequisito = tipoRequisito;
            this.Bookmark = Bookmark;
            this.Grupo = Grupo;
            this.OpcaoRequisito = Opcoes;
            this.MascaraEntrada = MascaraEntrada;
            this.Obrigatorio = Obrigatorio;
            this.Size = size;
            this.InText = inText;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        [Display(Name = "Nome do requisito")]
        [Required(ErrorMessage = "O Campo Nome do requisito é obrigatório")]
        public string NomeRequisito { get; set; }
        [Display(Name = "Elemento de entrada de dados")]
        [Required(ErrorMessage = "O Campo Elemento de entrada de dados é obrigatório")]
        public EnumElementoHTMLRequisito ElementoHTMLRequisito { get; set; }
        [Display(Name = "Formato de entrada de dados")]
        [Required(ErrorMessage = "O Campo Formato de entrada de dados é obrigatório")]
        public EnumTipoRequisito TipoElementoHTMLRequisito { get; set; }
        [Display(Name = "Bookmark/Indicador")]
        [Required(ErrorMessage = "O Campo Indicador é obrigatório")]
        public string Bookmark { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O Campo Descricao é obrigatório")]
        public string Descricao { get; set; }
        [Display(Name = "Grupo")]
        [Required(ErrorMessage = "O Campo Grupo é obrigatório")]
        public EnumGrupoRequisito Grupo { get; set; }
        [Display(Name = "Mascara de entrada de dados")]
        public string MascaraEntrada { get; set; }
        [Display(Name = "Obrigatório")]
        public bool Obrigatorio { get; set; }
        public int Size { get; set; }
        public bool InText { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - ({1})", NomeRequisito, Bookmark);
        }

        public void Recuperar()
        {
            DataHoraExclusao = null;
        }

        public virtual ICollection<OpcaoRequisito> OpcaoRequisito { get; set; }
        public virtual ICollection<RequisitoDeUsuario> RequisitoDeUsuario { get; set; }
        public virtual ICollection<RequisitoDeDocumento> RequisitoDeDocumento { get; set; }
    }
}
