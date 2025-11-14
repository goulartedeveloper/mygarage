using Garage.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Garage.Domain;
using Garage.Infrastructure;
using Garage.Infrastructure.Interfaces;
using Garage.Worker.Common;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<ICurrentUser, WorkerUser>();

builder.Services.AddDomainModule(builder.Configuration, builder.Environment.EnvironmentName, isWorker: true);
builder.Services.AddInfrastructureModule(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
