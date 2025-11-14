using Garage.Domain;
using Garage.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Garage.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;
using Garage.Infrastructure.Interfaces;
using Garage.Api.Common;
using Garage.Infrastructure.Database;

namespace Garage.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOpenApi();

        if (builder.Environment.EnvironmentName == "Testing")
        {
            var path = Directory.GetCurrentDirectory();
            builder.Configuration.AddJsonFile(Path.Combine(path, "appsettings.json"));
        }
        else if (builder.Environment.IsDevelopment())
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
            var externalPath = Path.Combine(root!.FullName, "appsettings.json");

            builder.Configuration
                .AddJsonFile(externalPath, optional: false, reloadOnChange: true);
        }

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

        builder.Services.AddInfrastructureModule(builder.Configuration);
        builder.Services.AddDomainModule(builder.Configuration, builder.Environment.EnvironmentName);

        builder.Services.AddControllers();

        var jwtKey = builder.Configuration["Jwt:Key"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<GarageContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();

    }
}
