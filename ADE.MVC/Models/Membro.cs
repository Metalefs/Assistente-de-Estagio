using System;
using System.Collections.Generic;
using System.Text;

namespace ADE.Apresentacao.Models
{
    public class Membro
    {
        public string Nome { get; set; }
        public string ImgSrc { get; set; }
        public string Funcao { get; set; }
        public string LinkedIn { get; set; }

        public Membro(string nome, string imgSrc, string funcao, string linkedIn)
        {
            Nome = nome;
            ImgSrc = imgSrc;
            Funcao = funcao;
            LinkedIn = linkedIn;
        }
    }
}
