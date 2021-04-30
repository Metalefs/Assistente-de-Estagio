using ADE.Dominio.Models;
using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ADE.Utilidades.Handlers
{
    public static class FileHandler : object
    {
        public static bool ArquivoValido(IFormFile file)
        {
            return file.Length < Constants.Constants.TamanhoMaximoArquivo && file.ContentType == Constants.Constants.DocXMimeType || file.Length < Constants.Constants.TamanhoMaximoArquivo && file.ContentType == Constants.Constants.PDFMimeType ? true : false;
        }

        public static bool ArquivoDocXValido(IFormFile file)
        {
            return file.Length < Constants.Constants.TamanhoMaximoArquivo && file.ContentType == Constants.Constants.DocXMimeType ? true : false;
        }

        public static bool ArquivoDocXValido(FileInfo file)
        {
            return file.Length < Constants.Constants.TamanhoMaximoArquivo && file.Extension == "."+Constants.Constants.DocXFormat ? true : false;
        }

        public static string GerarCaminhoDiretorioDocumento(Instituicao instituicao, Curso curso, Documento documento, string WebRootPath)
        {
            return $"/Documents/{instituicao.Identificador}-{instituicao.Nome}/{curso.Identificador}-{curso.NomeCurso}";
        }

        public static string GerarCaminhoDocumento(Documento documento,string DirectoryPath, EnumFormatoDocumento formato)
        {
            return $"{DirectoryPath}/{documento.TituloDocumento}.{Enum.GetName(typeof(EnumFormatoDocumento), formato)}";
        }

        public static string GerarCaminhoDocumentoSemExtensao(Documento documento, string DirectoryPath)
        {
            return $"{DirectoryPath}/{documento.TituloDocumento}";
        }

        //public static string ObterCaminhoDocumento(Documento documento,string WebRootPath, EnumFormatoDocumento formato)
        //{
        //    return System.IO.Path.Combine(WebRootPath, $"{documento.CaminhoDocumento}.{Enum.GetName(typeof(EnumFormatoDocumento), formato)}");
        //}

    }
}
