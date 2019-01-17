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
        public readonly u911430744_estgContext _u911430744_estgContext;

        public void CreateDocument(string[] args)
        {
            List<DocumentoEstagio> dadosAluno = JsonConvert.DeserializeObject<List<DocumentoEstagio>>(args[0]);

            int size = dadosAluno.Count;

            Console.WriteLine(dadosAluno[0].name);

            string filePath = args[1];

            Application app = new word.Application();
            Document doc = app.Documents.Open(filePath + ".doc");

            for (int i = 0; i < size; i++)
            {
                FindAndReplace(app, '"' + dadosAluno[0].name + '"', '"' + dadosAluno[0].value + '"');
            }
            doc.SaveAs2(filePath + ".pdf", word.WdSaveFormat.wdFormatPDF);
        }
        // public List<Documento> ListAll()
        // {
        //    _context.Documento.ToList();
        //}

        //  public List<Documento> ObterRequisitos(int id)
        //  {
        //      _context.requisitos.Where(x => x.IdDocumento = id);
        // }


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
  