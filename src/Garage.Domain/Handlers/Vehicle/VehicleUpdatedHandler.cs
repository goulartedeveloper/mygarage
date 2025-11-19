using System;
using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Rebus.Handlers;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleUpdatedHandler : IHandleMessages<VehicleUpdatedMessage>
    {
        public Task Handle(VehicleUpdatedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
