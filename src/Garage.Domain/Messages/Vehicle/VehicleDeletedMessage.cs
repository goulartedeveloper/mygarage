using Garage.Domain.Models;

namespace Garage.Domain.Messages.Vehicle
{
    public class VehicleDeletedMessage : VehicleModel, IBaseMessage
    {
        public string UserId { get; set; }
    }
}
