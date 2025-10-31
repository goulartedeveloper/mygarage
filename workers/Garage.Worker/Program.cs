using Garage.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Garage.Domain;
using Garage.Infrastructure;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDomainModule(builder.Configuration, isWorker: true);
builder.Services.AddInfrastructureModule(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
