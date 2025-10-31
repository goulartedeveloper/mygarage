using System;
using FluentValidation;
using Garage.Domain.Models;

namespace Garage.Domain.Validators
{
    public class VehicleValidator : AbstractValidator<VehicleModel>
    {
        public VehicleValidator()
        {
            RuleFor(vehicle => vehicle.Make)
                .NotEmpty()
                .WithMessage("Make is required.");

            RuleFor(vehicle => vehicle.Plate)
                .NotEmpty()
                .WithMessage("Plate is required.");

            RuleFor(vehicle => vehicle.Model)
                .NotEmpty()
                .WithMessage("Model is required.");

            RuleFor(vehicle => vehicle.Year)
                .InclusiveBetween(1886, DateTime.Now.Year)
                .WithMessage("Year must be between 1886 and the current year.");
        }
    }
}