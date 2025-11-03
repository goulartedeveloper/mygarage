using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Garage.Infrastructure.Entities;
using Garage.Tests.Data;
using Garage.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Tests.Integration.Controllers
{
    public class VehicleControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly GarageContext _context;
        private readonly HttpClient _client;

        public VehicleControllerTests(CustomWebApplicationFactory factory)
        {
            _context = factory.Services.GetRequiredService<GarageContext>();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ValidModel_ReturnAccepted()
        {
            // Arrange
            var url = "api/vehicle";
            var model = DataGenerator.CreateVehicleModel();

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var content = await response.Content.ReadAsStringAsync();
            var vehicle = _context.Vehicles.FirstOrDefault(x => x.Model == model.Model && x.Plate == x.Plate);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(vehicle);
        }

        [Fact]
        public async Task GetById_ExistVehicle_ReturnOk()
        {
            // Arrange
            var url = "api/vehicle";
            var vehicle = DataGenerator.CreateVehicle();
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"{url}/{vehicle.Id}");
            var vehicleModel = await response.Content.ReadFromJsonAsync<VehicleModel>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(vehicleModel);
        }
    }
}
