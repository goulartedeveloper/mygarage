using System;
using System.Threading.Tasks;
using FluentValidation;
using Garage.Domain.Interfaces;
using Garage.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class VehicleController : BaseController<VehicleModel>
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService, IValidator<VehicleModel> validator)
         : base(validator)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleModel vehicle)
        {
            var action = await ValidateAsync(vehicle);

            if (action is not null)
                return action;

            await _vehicleService.Create(vehicle);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VehicleModel vehicle)
        {
            var action = await ValidateAsync(vehicle);

            if (action is not null)
                return action;

            await _vehicleService.Update(vehicle);
            return Accepted();
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
