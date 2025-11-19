using FluentValidation;
using Garage.Domain.Interfaces;
using Garage.Domain.Services;
using Garage.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Rebus.Config;
using Microsoft.Extensions.Configuration;
using Rebus.Routing.TypeBased;
using Garage.Domain.Messages.Vehicle;
using Rebus.Transport.InMem;
using Garage.Domain.Common;

namespace Garage.Domain
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services,
            IConfiguration configuration,
            string environment,
            bool isWorker = false)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var isDevelopment = environment == "Development";
            var isTesting = environment == "Testing";

            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IGarageService, GarageService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IValidator<Models.VehicleModel>, VehicleValidator>();
            services.AddScoped<IValidator<Models.GarageModel>, GarageValidator>();

            var rabbitMQ = configuration.GetConnectionString("RabbitMQ");

            if (isTesting || isDevelopment)
            {
                var network = new InMemNetwork();

                services.AddRebus((config, provider) => config
                    .Logging(l => l.Serilog())
                    .Transport(t => t.UseInMemoryTransport(network, "garage"))
                    .Routing(r => r.TypeBased()
                        .Map<VehicleCreatedMessage>("garage")
                        .Map<VehicleUpdatedMessage>("garage")
                        .Map<VehicleDeletedMessage>("garage")
                    ));

            }
            else
            {
                services.AddRebus(config => config
                    .Logging(l => l.Serilog())
                    .Transport(t => t.UseRabbitMqAsOneWayClient(rabbitMQ))
                    .Routing(r => r.TypeBased()
                        .Map<VehicleCreatedMessage>(RouterKeys.VehicleCreatedKey)
                        .Map<VehicleUpdatedMessage>(RouterKeys.VehicleUpdatedKey)
                        .Map<VehicleDeletedMessage>(RouterKeys.VehicleDeletedKey)
                    ));
            }

            if (isWorker || isDevelopment)
            {
                services.AutoRegisterHandlersFromAssemblyOf<VehicleCreatedMessage>();
                services.AutoRegisterHandlersFromAssemblyOf<VehicleUpdatedMessage>();
                services.AutoRegisterHandlersFromAssemblyOf<VehicleDeletedMessage>();
            }
        }
    }
}
