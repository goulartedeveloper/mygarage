using Bogus;
using Garage.Domain.Models;
using Garage.Infrastructure.Entities;

namespace Garage.Tests.Data
{
    public class DataGenerator
    {
        public static VehicleModel CreateVehicleModel(string plate = null)
        {
            return new Faker<VehicleModel>()
                .RuleFor(v => v.Plate, f => plate ?? f.Vehicle.Vin().Substring(0, 7).ToUpper())
                .RuleFor(v => v.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(v => v.Model, f => f.Vehicle.Model())
                .RuleFor(v => v.Year, f => f.Date.Past(30).Year)
                .RuleFor(v => v.Color, f => f.Commerce.Color())
                .Generate();
        }

        public static Vehicle CreateVehicle(string userId, string plate = null)
        {
            return new Faker<Vehicle>()
                .RuleFor(v => v.Plate, f => plate ?? f.Vehicle.Vin().Substring(0, 7).ToUpper())
                .RuleFor(v => v.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(v => v.Model, f => f.Vehicle.Model())
                .RuleFor(v => v.Year, f => f.Date.Past(30).Year)
                .RuleFor(v => v.Color, f => f.Commerce.Color())
                .RuleFor(v => v.UserId, _ => userId)
                .Generate();
        }
    }
}
