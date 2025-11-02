using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Garage.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Garage.Tests.Integration.Controllers
{
    public class VehicleControllerTests : IClassFixture<TestFixture>
    {
        private readonly GarageContext _context;
        private readonly HttpClient _client;

        public VehicleControllerTests(TestFixture testFixture)
        {
            _context = testFixture.ServiceProvider.GetRequiredService<GarageContext>();
            _client = new HttpClient();
        }

        [Fact]
        public async Task Post_ValidModel_ReturnAccepted()
        {
            // Arrange
            var url = "http://localhost:5001/api/vehicle";
            var model = new VehicleModel
            {
                Plate = "TEST",
                Make = "VW",
                Model = "GOL",
                Year = 2000,
                Color = "Vermelho"
            };

            // Act
            var response = await _client.PostAsJsonAsync(url, model);
            var content = await response.Content.ReadAsStringAsync();
            var vehicle = _context.Vehicles.FirstOrDefault(x => x.Model == model.Model && x.Plate == x.Plate);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(vehicle);
        }
    }
}
