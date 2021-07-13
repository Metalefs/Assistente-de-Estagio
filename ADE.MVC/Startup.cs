using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Assistente_de_Estagio.Data;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using ADE.Aplicacao.Services;
using ADE.Dominio.Models.Individuais;
using WebEssentials.AspNetCore.Pwa;
using Assistente_de_Estagio.Areas.Identity.CustomIdentityModels;
using ADE.Dominio.Interfaces;
using ADE.Infra.Data.Repository;
using ADE.Utilidades.Seguranca;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Assistente_de_Estagio
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            byte[] encKey = Encoding.UTF8.GetBytes(Configuration.GetSection("Schlussel").Value);
            byte[] encryptedString = Convert.FromBase64String(Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
            string decryptedString = Configuration.GetSection("ConnectionStrings")["DefaultConnection2"];
            string connectionString = Criptografia.Decriptografar(encryptedString, encKey);
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(decryptedString, ServerVersion.AutoDetect(decryptedString)), ServiceLifetime.Scoped);
            services.AddIdentity<UsuarioADE, IdentityRole>()
            .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 20;
                options.Lockout.AllowedForNewUsers = false;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                o.AreaViewLocationFormats.Add("/Areas/{2}/Pages/{1}/{0}.cshtml");
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{1}/{0}.cshtml");
                o.AreaViewLocationFormats.Add("/Areas/Shared/{0}.cshtml");
            });

            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddTransient<DbContext, ApplicationDbContext>();
            services.AddScoped<RoleServices>();
            services.AddScoped<AuthMessageSender>();

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration.GetSection("Secure")["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration.GetSection("Secure")["Authentication:Google:ClientSecret"];
            //});

            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration.GetSection("Secure")["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = Configuration.GetSection("Secure")["Authentication:Facebook:AppSecret"];
            //});

            services.AddAuthentication(o =>
            {
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".ADE";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });
            services.AddMvc(options => { options.MaxModelValidationErrors = 50; options.EnableEndpointRouting = false; options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((_) => "Algum campo obrigatorio estava nulo."); })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddMvcOptions(MvcOptions => MvcOptions.EnableEndpointRouting = false).AddSessionStateTempDataProvider()
            .AddRazorPagesOptions(options =>
            {
                //options.AllowAreas = true;
                options.Conventions.AuthorizeAreaFolder("Administracao", "/");
            });

            services.AddControllersWithViews();

            services.AddRazorPages().AddNewtonsoftJson();
            services.AddProgressiveWebApp(new PwaOptions()
            {
                RegisterServiceWorker = false
            });

            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Index}/{id?}"
            //    );
            //    routes.MapRoute(
            //     name: "areas",
            //     template: "{area:exists}/{controller=Account}/{action=Index}/{id?}"
            //   );
            //});

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Principal",
                    pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Acesso",
                    pattern: "{area:exists}/{controller=Acesso}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                   name: "areas",
                   areaName: "Administracao",
                   pattern: "{area:exists}/{controller=Administracao}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "areas",
                 areaName: "Ajuda",
                 pattern: "{area:exists}/{controller=Ajuda}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

        }
    }
}
