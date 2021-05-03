
namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public class ErrorPageViewModel
    {
        public string ErrorMessage { get; set; }
        public Imagem Imagem { get; set; }
        public int StatusCode { get; set; }

        public ErrorPageViewModel(string errorMessage, int statusCode, Imagem imagem)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            Imagem = imagem;
        }
    }
}
