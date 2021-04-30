using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ADE.Dominio.Interfaces;
namespace ADE.Dominio.Models
{
    public partial class TermoCompromisso : ModeloBase, IRecuperavel, IModeloADE
    {
        public TermoCompromisso()
        {
        }

        public TermoCompromisso(string titulo, string termos, string versao)
        {
            Titulo = titulo;
            Termos = termos;
            Versao = versao;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get; set; }
        [Display(Name = "Titulo do Termo")]
        [Required(ErrorMessage = "O Campo Titulo do Termo é obrigatório")]
        public string Titulo { get; set; }
        [Display(Name = "Conteúdo do termo")]
        [Required(ErrorMessage = "O Conteúdo do termo é obrigatório")]
        public string Termos { get; set; }
        [Display(Name = "Versão do termo")]
        public string Versao { get; set; }
       
        public void Recuperar()
        {
            DataHoraExclusao = null;
        }
    }
}
