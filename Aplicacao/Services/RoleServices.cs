using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ADE.Dominio.Models.Individuais;
using ADE.Dominio.Models.Enums;
using ADE.Utilidades.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;

namespace ADE.Aplicacao.Services
{
    public class RoleServices
    {
        private readonly UserManager<UsuarioADE> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleServices(UserManager<UsuarioADE> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration, IHostingEnvironment env)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UsuarioADE>>();
            string[] roleNames = { EnumTipoUsuario.Admin.GetDescription(), EnumTipoUsuario.Membro.GetDescription(), EnumTipoUsuario.CriadorConteudo.GetDescription() };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            byte[] Icone = IconPadrao(env);

            var poweruser = new UsuarioADE
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"],
                IdCurso = 1,
                IdInstituicao = 1,
                TipoRecebimentoNotificacao = EnumTipoRecebimentoNotificacao.Geral,
                Logo = Icone
            };
            var poweruser2 = new UsuarioADE
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail2"],
                Email = Configuration.GetSection("UserSettings")["UserEmail2"],
                IdCurso = 1,
                IdInstituicao = 1,
                TipoRecebimentoNotificacao = EnumTipoRecebimentoNotificacao.Geral,
                Logo = Icone
            };

            List<UsuarioADE> pUsers = new List<UsuarioADE>()
            {
                poweruser,
                poweruser2
            };

            foreach(UsuarioADE puser in pUsers)
            {
                 UsuarioADE usario = await UserManager.FindByEmailAsync(puser.Email);

                if (usario == null)
                {
                    string userPassword = puser.Email == Configuration.GetSection("UserSettings")["UserEmail2"] ? 
                        Configuration.GetSection("UserSettings")["UserPassword2"] :
                        Configuration.GetSection("UserSettings")["UserPassword"];

                    var createPowerUser = await UserManager.CreateAsync(puser, userPassword);

                    if (createPowerUser.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(puser, EnumTipoUsuario.Admin.GetDescription());
                        await UserManager.AddToRoleAsync(puser, EnumTipoUsuario.CriadorConteudo.GetDescription());

                        string token = await UserManager.GenerateEmailConfirmationTokenAsync(puser);
                        await UserManager.ConfirmEmailAsync(puser, token);
                    }
                }
            }
        }

        private static byte[] IconPadrao(IHostingEnvironment env)
        {
            byte[] Logo;
            string ImagePath;
            if (env.IsDevelopment())
            {
                ImagePath = Path.Combine(env.WebRootPath, "Images", "xxxhdpi.png");
            }
            else
            {
                ImagePath = Path.Combine(env.WebRootPath, "Images", "xxxhdpi.png");
            }
            using (Stream fs = new FileStream(ImagePath, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Logo = ms.ToArray();
                }
            }
            return Logo;
        }
    }
}
