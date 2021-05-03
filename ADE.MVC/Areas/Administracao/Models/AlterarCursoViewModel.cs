using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Assistente_de_Estagio.Areas.Administracao.Models
{
    public class AlterarCursoViewModel
    {
        [Required]
        public Curso Curso { get; set; }
        public List<Instituicao> Instituicoes { get; set; }
        public List<AreaEstagioCurso> AreasEstagio { get; set; }
        [Required]
        public int Selection { get; set; }

        public AlterarCursoViewModel()
        {
        }

        public AlterarCursoViewModel(Curso Curso, int selection, List<Instituicao> Instituicoes, List<AreaEstagioCurso> areasEstagio)
        {
            this.Instituicoes = Instituicoes;
            this.Curso = Curso;
            this.Selection = selection;
            this.AreasEstagio = areasEstagio;
        }
    }
}
