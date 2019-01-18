using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assistente_de_Estagio.Models;
using Assistente_de_Estagio.Services;

namespace Assistente_de_Estagio.Controllers
{
    public class HomeController : Controller
    {
        public readonly DocumentoServices _documentoServices;

        public HomeController(DocumentoServices documentoServices)
        {
            _documentoServices = documentoServices;
        }
        [HttpGet]
        public IActionResult Index()
        {            
            ViewBag.ListaRequisitos = _documentoServices.ObterRequisitos(1);
            ViewBag.Caminho = _documentoServices.ObterCaminho(1);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public FileResult Download(string dadosJson,int id)
        {
            string[] caminhoDoc = _documentoServices.ObterCaminho(id);
            string caminhoPDF = _documentoServices.CreateDocument(dadosJson,caminhoDoc);
            var fileName = $"{caminhoPDF}";
            var filepath = $"Downloads/{fileName}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }
    }
}
