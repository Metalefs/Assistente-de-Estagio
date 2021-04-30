using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ADE.Apresentacao.Areas.Shared;
using ADE.Dominio.Models.Individuais;
using ADE.Utilidades.Extensions;
using Assistente_de_Estagio.Areas.Acesso.Models;
using Assistente_de_Estagio.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ADE.Apresentacao.Controllers
{
    public class ErrorController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;

        public ErrorController(
            IHostingEnvironment hostingEnvironment,
            ApplicationDbContext context,
            UserManager<UsuarioADE> userManager
            ) : base(new Infra.Data.UOW.UnitOfWork(context), userManager)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("/Error/{statusCode}")]
        public async Task<IActionResult> HttpStatusCodeHandler(int statusCode, string message = null)
        {
            if(statusCode == 0)
            {
                return RedirectToAction("Index", "Account", new { });
            }
            string ErrorMessage = await ErrorMessageForStatusCode(statusCode, message);
            Imagem image = await ErrorImageForStatusCode(statusCode);
            ErrorPageViewModel Model = new ErrorPageViewModel(ErrorMessage, statusCode, image);
            await LogErrorStatus(statusCode);
            return View("ErrorPage", Model);
        }

        private async Task LogErrorStatus(int statusCode)
        {
            await LogError($"StatusCode: {statusCode}", "LogErrorStatus", Dominio.Models.Enums.EnumTipoLog.ErroInterno);
        }

        private async Task<string> ErrorMessageForStatusCode(int statusCode, string message = null)
        {
            if (Enum.TryParse("Status" + statusCode.ToString(), out EnumErrorMessages errorMessage))
            {
                try
                {
                    message = message ?? string.Empty;
                    return message +" - "+errorMessage.GetDescription();
                }
                catch(Exception ex)
                {
                    await LogError($"Erro ao obter descrição para o StatusCode {statusCode}, motivo: não foi possivel traduzir usar método GetDescription() - {ex.Message}", ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                    return "Ocorreu um erro na requisição.";
                }
            }
            else
            {
                await LogError($"Erro ao recuperar mensagem para o StatusCode {statusCode}, motivo: não foi possivel traduzir EnumErrorMessages", "GetErrorMessageForStatusCode",Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "Ocorreu um erro na requisição.";
            }
        }
        
        private async Task<Imagem> ErrorImageForStatusCode(int statusCode)
        {
            try
            {
                string ErrorPageImagesPath = $"Images/Backgrounds/ErrorPages/{statusCode.ToString()}";
                if (Directory.Exists(ErrorPageImagesPath))
                {
                    DirectoryInfo DiretorioDeImagens = new DirectoryInfo(Path.Combine(_hostingEnvironment.WebRootPath, ErrorPageImagesPath));
                    FileInfo[] Arquivos = DiretorioDeImagens.GetFiles();
                    List<Imagem> Imagens = new List<Imagem>();
                    foreach (FileInfo arquivo in Arquivos)
                    {
                        string ImagePath = Path.Combine(ErrorPageImagesPath, arquivo.Name);
                        Imagem Imagem = new Imagem(arquivo.Name, ImagePath);
                        Imagens.Add(Imagem);
                    }
                    return Imagens.LastOrDefault();
                } else
                {
                    return new Imagem("", "");
                }
            }
            catch (Exception ex)
            {
                await LogError($"Erro ao obter imagem para o StatusCode {statusCode}, motivo: {ex.Message}", ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return new Imagem("", "");
            }
        }

        [HttpGet]
        public async Task<string> GetErrorImage(int statusCode)
        {
            try
            {
                string ErrorPageImagesPath = $"Images/Backgrounds/ErrorPages/{statusCode.ToString()}";
                if (Directory.Exists(ErrorPageImagesPath))
                {
                    DirectoryInfo DiretorioDeImagens = new DirectoryInfo(Path.Combine(_hostingEnvironment.WebRootPath, ErrorPageImagesPath));
                    FileInfo[] Arquivos = DiretorioDeImagens.GetFiles();
                    List<Imagem> Imagens = new List<Imagem>();
                    foreach (FileInfo arquivo in Arquivos)
                    {
                        string ImagePath = Path.Combine(ErrorPageImagesPath, arquivo.Name);
                        Imagem Imagem = new Imagem(arquivo.Name, ImagePath);
                        Imagens.Add(Imagem);
                    }
                    return Imagens.LastOrDefault().Caminho;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                await LogError($"Erro ao obter imagem para o StatusCode {statusCode}, motivo: {ex.Message}", ex.Source, Dominio.Models.Enums.EnumTipoLog.ErroInterno);
                return "";
            }
        }
    }
}