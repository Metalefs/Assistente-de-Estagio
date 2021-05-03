using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using ADE.Infra.Data.UOW;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Views.Shared.Components.InstituicaoSelectViewComponent
{
    public class InstituicaoSelectViewComponent : ViewComponent
    {
        UnitOfWork unitOfWork;
        ServicoInstituicao ServicoInstituicao;
        public InstituicaoSelectViewComponent(ApplicationDbContext context)
        {
            unitOfWork = new UnitOfWork(context);
            ServicoInstituicao = new ServicoInstituicao(ref unitOfWork);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Instituicao> instituicoes = await ServicoInstituicao.ListarAsync();
            return View(instituicoes);
        }
    }
}
