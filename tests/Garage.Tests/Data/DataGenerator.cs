using System;
using Bogus;
using Garage.Domain.Models;
using ET = Garage.Infrastructure.Entities;

namespace Garage.Tests.Data
{
    public class DataGenerator
    {
        public static VehicleModel CreateVehicleModel(Guid garageId, string plate = null)
        {
            return new Faker<VehicleModel>()
                .RuleFor(v => v.Plate, f => plate ?? f.Vehicle.Vin().Substring(0, 7).ToUpper())
                .RuleFor(v => v.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(v => v.Model, f => f.Vehicle.Model())
                .RuleFor(v => v.Year, f => f.Date.Past(30).Year)
                .RuleFor(v => v.Color, f => f.Commerce.Color())
                .RuleFor(v => v.GarageId, _ => garageId)
                .Generate();
        }

        public static ET.Vehicle CreateVehicle(string userId, Guid garageId, string plate = null)
        {
            return new Faker<ET.Vehicle>()
                .RuleFor(v => v.Plate, f => plate ?? f.Vehicle.Vin().Substring(0, 7).ToUpper())
                .RuleFor(v => v.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(v => v.Model, f => f.Vehicle.Model())
                .RuleFor(v => v.Year, f => f.Date.Past(30).Year)
                .RuleFor(v => v.Color, f => f.Commerce.Color())
                .RuleFor(v => v.UserId, _ => userId)
                .RuleFor(v => v.GarageId, _ => garageId)
                .Generate();
        }

        public static GarageModel CreateGarageModel()
        {
            return new Faker<GarageModel>()
                .RuleFor(g => g.Name, f => f.Name.FirstName())
                .Generate();
        }

        public static ET.Garage CreateGarage(string userId)
        {
            return new Faker<ET.Garage>()
                .RuleFor(g => g.Name, f => f.Name.FirstName())
                .RuleFor(g => g.UserId, _ => userId)
                .Generate();
        }
    }
}
