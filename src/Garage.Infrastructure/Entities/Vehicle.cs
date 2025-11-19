using System;

namespace Garage.Infrastructure.Entities;

public class Vehicle : UserBase
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Plate { get; set; }
    public string Color { get; set; }

    public Guid GarageId { get; set; }
    public Garage Garage { get; set; }
}
