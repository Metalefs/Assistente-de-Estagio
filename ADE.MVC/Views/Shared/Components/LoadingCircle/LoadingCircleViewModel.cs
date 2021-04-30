namespace Assistente_de_Estagio.Views.Shared.Components.LoadingCircleViewModel
{
    public class LoadingCircle
    {
        public string Id { get; set; }
        public bool Closed { get; set; }

        public LoadingCircle(string id, bool closed)
        {
            Id = id;
            Closed = closed;
        }
    }
}