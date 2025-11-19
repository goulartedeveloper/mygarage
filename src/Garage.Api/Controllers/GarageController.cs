using System;
using System.Threading.Tasks;
using FluentValidation;
using Garage.Domain.Interfaces;
using Garage.Domain.Models;
using Garage.Domain.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GarageController : BaseController<GarageModel>
    {
        private readonly IGarageService _garageService;

        public GarageController(IGarageService garageService, IValidator<GarageModel> validator) : base(validator)
        {
            _garageService = garageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GarageModel garage)
        {
            var action = await ValidateAsync(garage);

            if (action is not null)
                return action;

            await _garageService.Create(garage);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GarageModel garage)
        {
            var action = await ValidateAsync(garage);

            if (action is not null)
                return action;

            await _garageService.Update(garage);
            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _garageService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var garage = await _garageService.GetById(id);
            return Ok(garage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var garages = await _garageService.GetAll();
            return Ok(garages);
        }
    }
}
