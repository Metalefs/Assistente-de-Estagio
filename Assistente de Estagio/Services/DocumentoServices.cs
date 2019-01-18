using Assistente_de_Estagio.Models;
using Microsoft.Office.Interop.Word;
using System;
using word = Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Services
{
    public class DocumentoServices
    {
        public readonly u2019_estgContext _context;

        public DocumentoServices(u2019_estgContext context)
        {
            _context = context;
        }

        public string CreateDocument(string dadosJson, string[] caminhoDocumento) // "[0]$dadosJson -- [1]$path
        {
            List<DocumentoEstagio> dadosAluno = JsonConvert.DeserializeObject<List<DocumentoEstagio>>(dadosJson); 

            int size = dadosAluno.Count;

            Console.WriteLine(dadosAluno[0].name);

            string filePath = caminhoDocumento[0];

            Application app = new word.Application();
            Document doc = app.Documents.Open(filePath + ".doc");

            for (int i = 0; i < size; i++)
            {
                FindAndReplace(app, '"' + dadosAluno[0].name + '"', '"' + dadosAluno[0].value + '"');
            }
            doc.SaveAs2($"Downloads/"+ caminhoDocumento[1] + ".pdf", word.WdSaveFormat.wdFormatPDF);

            return filePath+".pdf";
        }
         public List<Documento> ListAll()
         {
           return _context.Documento.ToList();
         }
        
        public List<Requisitos> ObterRequisitos(int id)
        {
            int i = 0;
            List<Requisitos> requisitos = new List<Requisitos>();
            var LRD = _context.RequisitoDeDocumento.Where(x => x.DocumentoIdDocumento == id).ToList();
            foreach (Requisitodedocumento RDD in LRD)
            {
                requisitos.AddRange(_context.Requisitos.Where(x => x.IdRequisito == LRD.ElementAt(i).RequisitosIdRequisito));
                i++;
            }
            return requisitos;

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
            object matchCase = false;
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
  