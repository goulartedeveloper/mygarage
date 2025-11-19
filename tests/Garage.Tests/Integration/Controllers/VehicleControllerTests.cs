using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Garage.Api.Responses;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Garage.Tests.Data;
using Garage.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Tests.Integration.Controllers
{
    public class VehicleControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly GarageContext _context;
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private string _userId = Guid.NewGuid().ToString();

        public VehicleControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _context = factory.Services.GetRequiredService<GarageContext>();
            _client = factory.CreateClient();
            var token = factory.GenerateToken(_userId);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task Post_ValidModel_ReturnAccepted()
        {
            // Arrange
            var url = "api/vehicle";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var model = DataGenerator.CreateVehicleModel(garage.Id);

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
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var vehicle = DataGenerator.CreateVehicle(_userId, garage.Id);
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
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var existingVehicle = DataGenerator.CreateVehicle(_userId, garage.Id);
            _context.Vehicles.Add(existingVehicle);
            await _context.SaveChangesAsync();
            var model = DataGenerator.CreateVehicleModel(garage.Id, plate: existingVehicle.Plate);

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Plate" && r.Message == "Already exists vehicle with this plate.");
        }

        [Fact]
        public async Task Delete_ExistVehicle_ReturnNoContent()
        {
            // Arrange
            var url = "api/vehicle";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var vehicle = DataGenerator.CreateVehicle(_userId, garage.Id);
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"{url}/{vehicle.Id}");
            var deletedVehicle = _context.Vehicles.FirstOrDefault(x => x.Id == vehicle.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(deletedVehicle);
        }

        [Fact]
        public async Task GetAll_VehiclesExist_ReturnOk()
        {
            // Arrange
            var url = "api/vehicle";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var vehicle = DataGenerator.CreateVehicle(_userId, garage.Id);
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync(url);
            var vehicles = await response.Content.ReadFromJsonAsync<VehicleModel[]>();
            var count = await _context.Vehicles.CountAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(vehicles);
            Assert.Equal(count, vehicles.Length);
        }

        [Fact]
        public async Task Put_ValidModel_ReturnAccepted()
        {
            // Arrange
            var url = "api/vehicle";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var vehicle = DataGenerator.CreateVehicle(_userId, garage.Id);
            vehicle.Year = 2000;
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            var model = DataGenerator.CreateVehicleModel(garage.Id, plate: vehicle.Plate);
            model.Id = vehicle.Id;
            model.Year = 2020;

            // Act
            var response = await _client.PutAsJsonAsync(url, model);
            var getResponse = await _client.GetAsync($"{url}/{vehicle.Id}");
            var updatedVehicle = await getResponse.Content.ReadFromJsonAsync<VehicleModel>();

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.NotNull(updatedVehicle);
            Assert.Equal(model.Model, updatedVehicle.Model);
            Assert.Equal(model.Plate, updatedVehicle.Plate);
            Assert.Equal(model.Year, updatedVehicle.Year);
        }

        [Fact]
        public async Task Put_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var url = "api/vehicle";
            var model = DataGenerator.CreateVehicleModel(Guid.NewGuid());
            model.Id = Guid.NewGuid();

            // Act
            var response = await _client.PutAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Id" && r.Message == "Vehicle not found with this id.");
        }

        [Fact]
        public async Task GetAll_InvalidToken_ReturnUnauthorized()
        {
            // Arrange
            var url = "api/vehicle";
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
