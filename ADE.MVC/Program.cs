using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using ADE.Aplicacao.Services;
using ADE.Utilidades.Seguranca;
using System.Text;

namespace Assistente_de_Estagio
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
            .UseStartup<Startup>()
             .ConfigureAppConfiguration((hostContext, config) =>
             {
                 var env = hostContext.HostingEnvironment;

                 var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");

                 config.AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: true) // When running using dotnet run
                 .AddJsonFile("SharedSettings.json", optional: true) // When app is published
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                 config.AddEnvironmentVariables();

             })
            .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var environment = services.GetRequiredService<IHostingEnvironment>();
                    RoleServices.CreateRoles(serviceProvider, configuration, environment).Wait();
                    SeedingService.Seed(serviceProvider, configuration, environment).Wait();
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while creating roles");
                }
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

    }
}