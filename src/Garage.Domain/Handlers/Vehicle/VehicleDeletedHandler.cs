using System;
using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Rebus.Handlers;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleDeletedHandler : IHandleMessages<VehicleDeletedMessage>
    {
        public Task Handle(VehicleDeletedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
