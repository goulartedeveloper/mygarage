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
    public class GarageControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly GarageContext _context;
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private string _userId = Guid.NewGuid().ToString();

        public GarageControllerTests(CustomWebApplicationFactory factory)
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
            var url = "api/garage";
            var model = DataGenerator.CreateGarageModel();

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var content = await response.Content.ReadAsStringAsync();
            var garage = _context.Garages.FirstOrDefault(x => x.Name == model.Name);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(garage);
        }

        [Fact]
        public async Task GetById_ExistGarage_ReturnOk()
        {
            // Arrange
            var url = "api/garage";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"{url}/{garage.Id}");
            var garageModel = await response.Content.ReadFromJsonAsync<GarageModel>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(garageModel);
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var url = "api/garage";
            var model = new GarageModel();

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Name" && r.Message == "Name is required.");
        }

        [Fact]
        public async Task Delete_ExistGarage_ReturnNoContent()
        {
            // Arrange
            var url = "api/garage";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"{url}/{garage.Id}");
            var deletedGarage = _context.Garages.FirstOrDefault(x => x.Id == garage.Id);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(deletedGarage);
        }

        [Fact]
        public async Task GetAll_GaragesExist_ReturnOk()
        {
            // Arrange
            var url = "api/garage";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync(url);
            var garages = await response.Content.ReadFromJsonAsync<GarageModel[]>();
            var count = await _context.Garages.CountAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(garages);
            Assert.Equal(count, garages.Length);
        }

        [Fact]
        public async Task Put_ValidModel_ReturnAccepted()
        {
            // Arrange
            var url = "api/garage";
            var garage = DataGenerator.CreateGarage(_userId);
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();
            var model = DataGenerator.CreateGarageModel();
            model.Id = garage.Id;
            model.Name = "Test";

            // Act
            var response = await _client.PutAsJsonAsync(url, model);
            var getResponse = await _client.GetAsync($"{url}/{garage.Id}");
            var updatedGarage = await getResponse.Content.ReadFromJsonAsync<GarageModel>();

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.NotNull(updatedGarage);
            Assert.Equal(model.Name, updatedGarage.Name);
        }

        [Fact]
        public async Task Put_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var url = "api/garage";
            var model = DataGenerator.CreateGarageModel();
            model.Id = Guid.NewGuid();

            // Act
            var response = await _client.PutAsJsonAsync(url, model);
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse[]>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Contains(result, r => r.Field == "Id" && r.Message == "Garage not found with this id.");
        }

        [Fact]
        public async Task GetAll_InvalidToken_ReturnUnauthorized()
        {
            // Arrange
            var url = "api/garage";
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
