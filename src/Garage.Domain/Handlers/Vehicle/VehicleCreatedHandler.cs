using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Garage.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleCreatedHandler : IHandleMessages<VehicleCreatedMessage>
    {
        private readonly ILogger<VehicleCreatedHandler> _logger;
        private readonly GarageContext _context;

        public VehicleCreatedHandler(ILogger<VehicleCreatedHandler> logger, GarageContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Handle(VehicleCreatedMessage message)
        {
            var user = await _context.Users.FindAsync(message.UserId);
            _logger.LogInformation("Vehicle {Make} - {Model} - {Year} created with success!", message.Make, message.Model, message.Year);
        }
    }
}
