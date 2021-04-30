
namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public class Imagem 
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }

        public Imagem() {}

        public Imagem(string _Nome,string _Caminho) {
            this.Nome = _Nome;
            this.Caminho = _Caminho;
        }
    }
}
