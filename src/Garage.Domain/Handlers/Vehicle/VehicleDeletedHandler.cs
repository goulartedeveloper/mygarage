using System;
using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Garage.Infrastructure.Database;
using Rebus.Handlers;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleDeletedHandler : BaseHandler<VehicleDeletedMessage>
    {
        public VehicleDeletedHandler(GarageContext garageContext) : base(garageContext)
        {
        }

        public override Task ExecuteAsync(VehicleDeletedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
