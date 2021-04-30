using ADE.Aplicacao.Services;
using ADE.Dominio.Models;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Assistente_de_Estagio.Views.Shared.Components.FormularioRequisitosBasicosViewComponent
{
    public class FormularioRequisitosBasicosViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoRequisito ServicoRequisito;
        ServicoRequisitoUsuario ServicoRequisitoUsuario;
        private ServicoUsuario servicoUsuario;
        UserManager<UsuarioADE> userManager;
        public FormularioRequisitosBasicosViewComponent(UserManager<UsuarioADE> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            unitOfWork = new UnitOfWork(context);
            ServicoRequisito = new ServicoRequisito(ref unitOfWork);
            ServicoRequisitoUsuario = new ServicoRequisitoUsuario(ref unitOfWork);
        }
        public async Task<IViewComponentResult> InvokeAsync(string tipo = "Default")
        {
            List<Requisito> lista = await ServicoRequisito.ListarAsync();
            string view = tipo;
            FormularioRequisitosBasicosVM model = new FormularioRequisitosBasicosVM(new List<Requisito>(),false);
            try
            {
                servicoUsuario = new ServicoUsuario(unitOfWork, userManager);
                UsuarioADE usuario = await servicoUsuario.ObterDetalhesUsuario(UserClaimsPrincipal);
                foreach (Requisito req in lista)
                {
                    req.RequisitoDeUsuario = await ServicoRequisitoUsuario.BuscarRequisitoUsuario(req.Identificador, usuario.Id);
                }
                model = new FormularioRequisitosBasicosVM(lista,usuario.Estagiando);
            }
            catch (System.Exception) { }
            return View(view, model);
        }

    }

    public class FormularioRequisitosBasicosVM
    {
        public List<Requisito> Lista { get; set; }
        public bool Estagiando { get; set; }

        public FormularioRequisitosBasicosVM(List<Requisito> lista, bool estagiando)
        {
            Lista = lista;
            Estagiando = estagiando;
        }
    }
}
