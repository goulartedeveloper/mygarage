using System;
using System.Threading.Tasks;
using Garage.Domain.Messages.Vehicle;
using Garage.Infrastructure.Database;

namespace Garage.Domain.Handlers.Vehicle
{
    public class VehicleUpdatedHandler : BaseHandler<VehicleUpdatedMessage>
    {
        public VehicleUpdatedHandler(GarageContext garageContext) : base(garageContext)
        {
        }

        public override Task ExecuteAsync(VehicleUpdatedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
