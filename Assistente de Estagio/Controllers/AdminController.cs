using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assistente_de_Estagio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assistente_de_Estagio.Services;
using Microsoft.AspNetCore.Hosting;
using Assistente_de_Estagio.Models.Enums;


namespace Assistente_de_Estagio.Controllers
{
    public class AdminController : Controller
    {

        public readonly DocumentoServices _documentoServices;
        public readonly UsuarioService _usuarioService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public readonly u2019_estgContext _context;

        public AdminController(UsuarioService usuarioService, DocumentoServices documentoServices, IHostingEnvironment hostingEnvironment, u2019_estgContext context)
        {
            _context = context;
            _usuarioService = usuarioService;
            _documentoServices = documentoServices;
            _hostingEnvironment = hostingEnvironment;
        }


        
        public ActionResult Index()
        {
            return View();
        }

        
        // GET: Documents/Create
        public ActionResult GerenciarDocumento()
        {
            return View();
        }
        // GET: Documents/Create
        public ActionResult CriarDocumento()
        {
            return View();
        }
        public ActionResult GerenciarCurso()
        {
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAction ([Bind("NomeEmail,senha")] string NomeEmail, string senha)
        {
            
                Prioridade acesso = _usuarioService.CheckUsuario(NomeEmail, senha);

                if(acesso == Prioridade.Usuario)
                {
                    return RedirectToAction(nameof(Index));
                }

                else if(acesso == Prioridade.Admin)
                {
                    return RedirectToAction(nameof(Tools));
                }
                
            
            ModelState.AddModelError("Erro-de-conexão", "Erro de conexão");
            ViewBag.Error = true;
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Edit/5
        public ActionResult Tools(int id)
        {
            return View();
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Documents/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}