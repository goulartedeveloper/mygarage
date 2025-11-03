using Microsoft.AspNetCore.Mvc.Testing;
using Garage.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Garage.Infrastructure.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Tests.Infrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddConfiguration(Configuration);
            });

            builder.ConfigureServices(services =>
            {
                // Remove o contexto padrão registrado pelo projeto
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<GarageContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<GarageContext>(options =>
                {
                    options.UseNpgsql(Configuration.GetConnectionString("GarageDatabase"));
                });



                // Adiciona um contexto apontando para um banco de teste isolado
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);
            return host;
        }
    }
}
