using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assistente_de_Estagio.Models;
using Assistente_de_Estagio.Services;
using Microsoft.AspNetCore.Hosting;
using Assistente_de_Estagio.Models;

namespace Assistente_de_Estagio.Controllers
{
    public class HomeController : Controller
    {
        public readonly DocumentoServices _documentoServices;
        private readonly IHostingEnvironment _hostingEnvironment;
        public readonly u2019_estgContext _context;

        public HomeController(DocumentoServices documentoServices,IHostingEnvironment hostingEnvironment, u2019_estgContext context)
        {
            _context = context;
            _documentoServices = documentoServices;
            _hostingEnvironment = hostingEnvironment;
        }
        

        [HttpGet]
        public IActionResult Index()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            List<Documento> docList = _context.Documento.ToList();
            ViewBag.ListaRequisitos = _documentoServices.ObterRequisitos(1);
            ViewBag.Caminho = _documentoServices.ObterCaminho(1);
            return View(docList);
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
        [HttpPost]
        public ActionResult Download(string dados, int idDocumento)
        {
            string[] caminhoDoc = _documentoServices.ObterCaminho(idDocumento);
            string caminhoPDF = _documentoServices.CreateDocument(dados, caminhoDoc);

            Jsonrequisitospreenchidos dadosJson = new Jsonrequisitospreenchidos(){DocumentoIdDocumento = idDocumento, DadosJson = dados };
            _context.Add(dadosJson);
            

            
            byte[] fileBytes = System.IO.File.ReadAllBytes(caminhoPDF);
            return File(fileBytes, "application/x-msdownload", caminhoPDF);
            


        }
    }
}
