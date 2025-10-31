using System;
using Garage.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("GarageDatabase");

            var config = new Action<DbContextOptionsBuilder>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddDbContext<GarageContext>(config);
        }
    }
}
