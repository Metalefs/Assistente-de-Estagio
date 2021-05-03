using ADE.Dominio.Models;
using Microsoft.AspNetCore.Http;

namespace ADE.Dominio.Interfaces.Utilidades
{
    public abstract partial class IFileHandler
    {
        public abstract bool ArquivoDocXValido(IFormFile file);
        public abstract string GerarCaminhoDiretorioDocumento(Documento documento, string WebRootPath);
        public abstract string GerarCaminhoDocumento(Documento documento, string WebRootPath, string Format);
        public abstract string ObterCaminhoDocumento(Documento documento, string WebRootPath, string formato);
    }
}
