using Garage.Domain.Models;

namespace Garage.Domain.Messages.Vehicle
{
    public class VehicleCreatedMessage : VehicleModel, IBaseMessage
    {
        public string UserId { get; set; }
    }
}
