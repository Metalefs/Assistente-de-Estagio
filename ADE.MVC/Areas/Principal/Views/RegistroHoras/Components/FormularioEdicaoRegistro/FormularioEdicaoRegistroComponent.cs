﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ADE.Dominio.Models;
using System;
using Assistente_de_Estagio.Areas.Principal.Models;

namespace Assistente_de_Estagio.Areas.Principal.UserHome.Components.FormularioEdicaoRegistroViewComponent
{
    public class FormularioEdicaoRegistroViewComponent : ViewComponent
    {
        public FormularioEdicaoRegistroViewComponent()
        {  }

        public async Task<IViewComponentResult> InvokeAsync(RegistroDeHoras registro)
        {
            return View(registro);
        }
    }
}
