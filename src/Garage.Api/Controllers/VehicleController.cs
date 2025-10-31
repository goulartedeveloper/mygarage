using System;
using System.Threading.Tasks;
using Garage.Domain.Interfaces;
using Garage.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleModel vehicle)
        {
            await _vehicleService.Create(vehicle);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VehicleModel vehicle)
        {
            await _vehicleService.Update(vehicle);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetById(id);
            return Ok(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAll();
            return Ok(vehicles);
        }
    }
}
