using ADE.Dominio.Models;
using System.Collections.Generic;
using System;
using ADE.Dominio.Interfaces;
using ADE.Dominio.Models.Individuais;

namespace Assistente_de_Estagio.Areas.Shared
{
    public class Mensagem
    {
        public int Identificador { get; set; }
        public string Conteudo { get; set; }
        public string Cabecalho { get; set; }
        public IModeloADE Entidade { get; set; }
        public string TipoEntidade { get; set; }
        public string IdAutor { get; set; }
        public string NomeAutor { get; set; }
        public DateTime Data { get; set; }
    }
}
