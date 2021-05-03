namespace Assistente_de_Estagio.Areas.Principal.Models
{
    public class CardApresentacaoViewmodel<T>
    {
        public T Item { get; set; }
        public string Texto { get; set; }
       
        public CardApresentacaoViewmodel(T Item)
        {
            this.Item = Item;
        }

        public CardApresentacaoViewmodel(T Item, string Texto)
        {
            this.Item = Item;
            this.Texto = Texto;
        }
    }
}
