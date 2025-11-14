using FluentValidation;
using Garage.Domain.Interfaces;
using Garage.Domain.Services;
using Garage.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Rebus.Config;
using Microsoft.Extensions.Configuration;
using Garage.Domain.Common;
using Garage.Domain.Handlers.Vehicle;
using Rebus.Routing.TypeBased;
using Garage.Domain.Messages.Vehicle;
using System;
using Rebus.Transport.InMem;

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

            services.AddScoped<IVehicleService, VehicleService>();

            services.AddScoped<IValidator<Models.VehicleModel>, VehicleValidator>();

            var rabbitMQ = configuration.GetConnectionString("RabbitMQ");

            if (isWorker)
            {
                services.AddRebus(config => config
                    .Logging(l => l.Serilog())
                    .Transport(t => t.UseRabbitMq(rabbitMQ, RouterKeys.VehicleCreatedKey)));
                services.AutoRegisterHandlersFromAssemblyOf<VehicleCreatedHandle>();
            }
            else
            {
                if (environment == "Testing")
                {
                    var network = new InMemNetwork();
                    services.AddRebus(config => config
                        .Logging(l => l.Serilog())
                        .Routing(r => r.TypeBased().Map<VehicleCreatedMessage>(RouterKeys.VehicleCreatedKey))
                        .Transport(t => t.UseInMemoryTransportAsOneWayClient(network)));
                }
                else
                {
                    services.AddRebus(config => config
                        .Logging(l => l.Serilog())
                        .Routing(r => r.TypeBased().Map<VehicleCreatedMessage>(RouterKeys.VehicleCreatedKey))
                        .Transport(t => t.UseRabbitMqAsOneWayClient(rabbitMQ)));
                }
            }
        }
    }
}
