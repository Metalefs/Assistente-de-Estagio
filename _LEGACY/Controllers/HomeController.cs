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
using System.Threading;
using System.IO;


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
        [RouteAttribute("/Index")]
        public IActionResult Selecao()
        {
            ViewBag.ListaRequisitos = _documentoServices.ObterRequisitos(1);
            ViewBag.Caminho = _documentoServices.ObterCaminho(1);
            ViewBag.Curso = _documentoServices.ListCursos();



            List<Documento> docList = _documentoServices.ListAll();
            var Cursos = _documentoServices.ListCursos();
            var Registros = _documentoServices.ListRegistros();
            var Usuarios = _documentoServices.ListUsuarios();

            

            DocumentoCursoRegistroAluno viewModel = new DocumentoCursoRegistroAluno() {Documentos = docList, Cursos = Cursos, Fichas= Registros, Usuarios = Usuarios };

            return View(viewModel);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<ActionResult> DownloadAsync(string dados, int idDocumento)
        {
            string[] caminhoDoc = _documentoServices.ObterCaminho(idDocumento);
          
            var caminhoPDF = _documentoServices.CreateDocument(dados, caminhoDoc);
            var DownloadPath = _hostingEnvironment.WebRootPath + "\\Downloads\\" + Convert.ToString(System.DateTime.Now.Ticks) +".pdf";
            
            if (DownloadPath == null)  
              return Content("filename not present");  

              var memory = new MemoryStream();  
              using (var stream = new FileStream(DownloadPath, FileMode.Open))  
              {  
                  await stream.CopyToAsync(memory);  
              }  
              memory.Position = 0;  
              
            
            Jsonrequisitospreenchidos dadosJson = new Jsonrequisitospreenchidos(){DocumentoIdDocumento = idDocumento, DadosJson = dados };
            _context.Add(dadosJson);
            
            return File(memory, "application/pdf", Path.GetFileName(DownloadPath)); ;


            
            /*byte[] fileBytes = System.IO.File.ReadAllBytes(DownloadPath);
            return File(fileBytes, "application/x-msdownload", DownloadPath);*/
            


        }
    }
}
