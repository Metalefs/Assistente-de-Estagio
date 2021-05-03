using System.Collections.Generic;
using System.Linq;

namespace ADE.Dominio.Models
{
    public class ComparacaoRequisitos
    {
        public List<string> RequisitosNovos { get; set; }
        public List<Requisito> RequisitosExistentes { get; set; }
        public List<Requisito> RequisitosCompativeis { get; set; }

        public bool IsValidForComparison()
        {
            return (RequisitosNovos.Count > 0 && RequisitosExistentes.Count > 0);
        }

        public void DiferencaNovosEExistentes()
        {
            List<string> Diferenca = RequisitosNovos.Except(RequisitosExistentes.Select(x => x.Bookmark).ToList()).ToList();
            RequisitosNovos = Diferenca;
        }

        public void ObterComparacaoRequisitosCompativeis()
        {
            List<Requisito> Compativeis = new List<Requisito>();
            foreach (string Bookmark in RequisitosNovos)
            {
                if (BookmarkPresenteEmRequisitosExistentes(Bookmark))
                {
                    Requisito RequisitoCompativel = ObterRequisitoCompativel(Bookmark);
                    Compativeis.Add(RequisitoCompativel);
                }
            }
            RequisitosCompativeis = Compativeis;
        }

        private Requisito ObterRequisitoCompativel(string Bookmark)
        {
            return RequisitosExistentes.Where(x => x.Bookmark == Bookmark).First();
        }

        private bool BookmarkPresenteEmRequisitosExistentes(string Bookmark)
        {
            return RequisitosExistentes.Any(x => x.Bookmark == Bookmark);
        }
    }
}
