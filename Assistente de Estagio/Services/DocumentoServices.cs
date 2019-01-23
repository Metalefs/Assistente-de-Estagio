using Assistente_de_Estagio.Models;
using Microsoft.Office.Interop.Word;
using System;
using word = Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Assistente_de_Estagio.Services
{
    public class DocumentoServices
    {
        public readonly u2019_estgContext _context;
        private IHostingEnvironment _env;

        
        public DocumentoServices(u2019_estgContext context, IHostingEnvironment env)
        {
            _env = env;
            _context = context;
        }

        public string CreateDocument(string dadosJson, string[] caminhoDocumento) // "[0]$dadosJson -- [1]$path
        {
            List<DocumentoEstagio> dadosAluno = JsonConvert.DeserializeObject<List<DocumentoEstagio>>(dadosJson); 

            int size = dadosAluno.Count;

            Console.WriteLine(dadosAluno[0].name);

            
            var filePath = System.IO.Path.Combine(_env.WebRootPath, caminhoDocumento[0]);
            var DownloadPath = _env.WebRootPath + "\\Downloads\\" + caminhoDocumento[1] + "2.pdf";

            Application app = new word.Application();
            Document doc = app.Documents.Open(filePath+"10.doc");

            for (int i = 0; i < size; i++)
            {
                FindAndReplace(app, dadosAluno[i].name , dadosAluno[i].value);
            }
            //string docPath = "~/Downloads/" + caminhoDocumento[1] + "2.pdf";
            doc.SaveAs2(DownloadPath, word.WdSaveFormat.wdFormatPDF);


            doc.Close();
            app.Quit();
            
            return DownloadPath;
        }
         public List<Documento> ListAll()
         {
           return _context.Documento.OrderBy(x => x.PosicaoDocumento).ToList();
         }
        public List<Curso> ListCursos()
        {
            return _context.Curso.ToList();
        }
        public string ObterRequisitos(int id)
        {
            int i = 0;
            List<Requisitos> requisitos = new List<Requisitos>();
            var LRD = _context.Requisitodedocumento.Where(x => x.DocumentoIdDocumento == id).ToList();
            foreach (Requisitodedocumento RDD in LRD)
            {
                requisitos.AddRange(_context.Requisitos.Where(x => x.IdRequisito == LRD.ElementAt(i).RequisitosIdRequisito));
                i++;
            }
            string inputJson = JsonConvert.SerializeObject(requisitos, Formatting.Indented);
            return inputJson;
        }
        public string[] ObterCaminho(int id)
        {
           List<Documento> documento = _context.Documento.Where(x => x.IdDocumento == id).ToList();
           string[] names = { documento.FirstOrDefault().CaminhoDocumento, documento.FirstOrDefault().TituloDocumento };
           return names;
        }

        private static void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }
    }
    class DocumentoEstagio
    {
        public string name;
        public string value;

        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
    
    }
}
  