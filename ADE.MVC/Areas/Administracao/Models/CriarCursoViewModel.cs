using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class CriarCursoViewModel
    {
        [Required]
        public Curso Curso { get; set; }
        public List<Instituicao> Instituicoes { get; set; }
        [Required]
        public string IdInstituicao { get; set; }
        
        public string SelectedInstituicao { get; set; }

        public CriarCursoViewModel()
        {
        }
        public CriarCursoViewModel(Curso curso, string selection, List<Instituicao> instituicoes)
        {
            Instituicoes = instituicoes;
            Curso = curso;
            SelectedInstituicao = selection;
        }

    }
}
