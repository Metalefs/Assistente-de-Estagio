using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class InformacaoInstituicao
    {
        public Instituicao Instituicao { get; set; }
        public int QuantidadeAlunosInstituicao;
        public int QuantidadeCursosInstituicao;
        public bool InstituicaoDoUsuario { get; set; }

        public InformacaoInstituicao(Instituicao instituicao, int quantidadeAlunosInstituicao, int quantidadeCursosInstituicao, bool instituicaoDoUsuario = false)
        {
            Instituicao = instituicao;
            QuantidadeAlunosInstituicao = quantidadeAlunosInstituicao;
            QuantidadeCursosInstituicao = quantidadeCursosInstituicao;
            InstituicaoDoUsuario = instituicaoDoUsuario;
        }
        public InformacaoInstituicao()
        {}
    }
}
