//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using ADE.Dominio.Models;
//using System;
//using Assistente_de_Estagio.Areas.Principal.Models;
//namespace Assistente_de_Estagio.Areas.Principal.Shared.Components.IconeAdministracaoViewComponent
//{
//    public class IconeAdministracaoViewComponent : ViewComponent
//    {
//        public int Id { get; set; }
//        public string Gerenciamento { get; set; }
//        public IconeAdministracaoViewComponent()
//        {  }

//        public IconeAdministracaoViewComponent(int id, string gerenciamento)
//        {
//            this.Id = id;
//            Gerenciamento = gerenciamento;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(int Id, string Gerenciamento)
//        {
//            IconeAdministracaoViewComponent model = new IconeAdministracaoViewComponent(Id, Gerenciamento);
//            return View(model);
//        }
//    }
//}
