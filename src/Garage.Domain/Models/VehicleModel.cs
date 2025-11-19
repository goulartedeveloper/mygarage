using System;

namespace Garage.Domain.Models
{
    public class VehicleModel
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public string UserId { get; set; }
        public Guid GarageId { get; set; }
    }
}
