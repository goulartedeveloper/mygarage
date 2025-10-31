using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleCreatedHandle : IHandleMessages<VehicleCreatedMessage>
    {
        private readonly ILogger<VehicleCreatedHandle> _logger;

        public VehicleCreatedHandle(ILogger<VehicleCreatedHandle> logger)
        {
            _logger = logger;
        }

        public Task Handle(VehicleCreatedMessage message)
        {
            _logger.LogInformation("Vehicle {Make} - {Model} - {Year} created with success!", message.Make, message.Model, message.Year);
            return Task.CompletedTask;
        }
    }
}
