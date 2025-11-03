using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Garage.Api.Responses;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
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

        [Fact]
        public async Task Post_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var url = "api/vehicle";
            var model = new VehicleModel();

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Make" && r.Message == "Make is required.");
            Assert.Contains(result, r => r.Field == "Plate" && r.Message == "Plate is required.");
            Assert.Contains(result, r => r.Field == "Model" && r.Message == "Model is required.");
            Assert.Contains(result, r => r.Field == "Color" && r.Message == "Color is required.");
            Assert.Contains(result, r => r.Field == "Year" && r.Message == "Year must be between 1886 and the current year.");
        }

        [Fact]
        public async Task Post_DuplicatePlate_ReturnBadRequest()
        {
            // Arrange
            var url = "api/vehicle";
            var existingVehicle = DataGenerator.CreateVehicle();
            _context.Vehicles.Add(existingVehicle);
            await _context.SaveChangesAsync();
            var model = DataGenerator.CreateVehicleModel(plate: existingVehicle.Plate);

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Plate" && r.Message == "Already exists vehicle with this plate.");
        }
    }
}
