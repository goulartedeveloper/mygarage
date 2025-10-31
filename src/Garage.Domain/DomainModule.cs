using FluentValidation;
using Garage.Domain.Interfaces;
using Garage.Domain.Services;
using Garage.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Domain
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IValidator<Models.VehicleModel>, VehicleValidator>();
        }
    }
}