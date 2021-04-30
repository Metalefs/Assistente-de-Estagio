using ADE.Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADE.Dominio.Models.Individuais
{
    public class Instituicao : ModeloBase, IColorCoded, IRecuperavel, IModeloADE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Identificador { get ; set ; }
        [Required(ErrorMessage = "O Campo Nome da Instituição é obrigatório")]
        [Display(Name = "Nome da Instituição")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Campo Descricao é obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O Campo Diretor da Instituição é obrigatório")]
        [Display(Name = "Diretor da Instituição")]
        public string Diretor { get; set; }
        [Display(Name = "Endereço da Instituição")]
        public string Endereco { get; set; }
        [Display(Name = "Telefone da Instituição")]
        public string Telefone { get; set; }
        [Display(Name = "E-mail da Instituição")]
        public string Email { get; set; }
        [Display(Name = "Website da Instituição")]
        public string Website { get; set; }
        [Display(Name = "Cor de exibição")]
        public string Color { get; set; }
        [Display(Name = "Logo da faculdade")]
        [Required(ErrorMessage ="O Campo Logo da faculdade é obrigatório")]
        [NotMapped]
        public IFormFile LogoFile { get; set; }
        public byte[] Logo { get; set; }
        public Instituicao() { }

        public Instituicao(string nome, string descricao, string diretor, string endereco, string telefone, string email, string website,string color, byte[] Logo)
        {
            Nome = nome;
            Descricao = descricao;
            Diretor = diretor;
            Endereco = endereco;
            Telefone = telefone;
            Email = email;
            Color = color;
            Website = website;
            this.Logo = Logo;

        }

        public Instituicao(string nome, string descricao, string diretor, string endereco, string telefone, string email, string color)
        {
            Nome = nome;
            Descricao = descricao;
            Diretor = diretor;
            Endereco = endereco;
            Telefone = telefone;
            Email = email;
            Color = color;
        }

        public void Clonar(Instituicao instituicao)
        {
            Nome = instituicao.Nome;
            Descricao = instituicao.Descricao;
            Diretor = instituicao.Diretor;
            Endereco = instituicao.Endereco;
            Telefone = instituicao.Telefone;
            Email = instituicao.Email;
            Color = instituicao.Color;
        }

        public override string ToString()
        {
            return Nome;
        }

        public void Recuperar()
        {
            DataHoraExclusao = null;
        }

        public virtual ICollection<Curso> Cursos { get; set; }
        public virtual ICollection<UsuarioADE> Usuarios { get; set; }
        public virtual ICollection<FAQ> FAQ { get; set; }
    }
}
