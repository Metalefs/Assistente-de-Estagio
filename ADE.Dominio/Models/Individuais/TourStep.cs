using ADE.Apresentacao.Models;
using ADE.Dominio.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ADE.Dominio.Models.Individuais
{
    public class TourStep : ModeloBase
    {
        [Key]
        public override int Identificador { get; set; }
        
        public int Posicao { get; set; }
        public string IdElemento { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Area { get; set; }
        public string Controlador { get; set; }
        public string View { get; set; }
        public TourStep()
        {
        }
        public TourStep(int posicao, string idElemento, string titulo, string conteudo, string area, string controlador, string view)
        {
            IdElemento = idElemento;
            Posicao = posicao;
            Titulo = titulo;
            Conteudo = conteudo;
            Area = area;
            Controlador = controlador;
            View = view;
        }

        public TourStep(int identificador, int posicao, string idElemento, string titulo, string conteudo,  string area, string controlador, string view)
        {
            Identificador = identificador;
            Posicao = posicao;
            Titulo = titulo;
            IdElemento = idElemento;
            Conteudo = conteudo;
            Area = area;
            Controlador = controlador;
            View = view;
        }

        public TourStepJs ToJs()
        {
            return new TourStepJs(IdElemento,Titulo,Conteudo);
        }
    }
}
