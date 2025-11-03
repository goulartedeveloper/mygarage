using System;
using FluentValidation;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Garage.Domain.Validators
{
    public class VehicleValidator : AbstractValidator<VehicleModel>
    {
        public VehicleValidator(GarageContext context)
        {
            RuleFor(vehicle => vehicle.Make)
                .NotEmpty()
                .WithMessage("Make is required.");

            RuleFor(vehicle => vehicle.Plate)
                .NotEmpty()
                .WithMessage("Plate is required.");

            RuleFor(vehicle => vehicle.Plate)
                .MustAsync(async (plate, cancellation) =>
                    !await context.Vehicles.AnyAsync(v => v.Plate == plate, cancellation))
                .When(vehicle => !string.IsNullOrWhiteSpace(vehicle.Plate) && vehicle.Id == Guid.Empty)
                .WithMessage("Already exists vehicle with this plate.");

            RuleFor(vehicle => vehicle.Model)
                .NotEmpty()
                .WithMessage("Model is required.");

            RuleFor(vehicle => vehicle.Color)
                .NotEmpty()
                .WithMessage("Color is required.");

            RuleFor(vehicle => vehicle.Year)
                .InclusiveBetween(1886, DateTime.Now.Year)
                .WithMessage("Year must be between 1886 and the current year.");

            RuleFor(vehicle => vehicle.Id)
                .MustAsync(async (id, cancellation) =>
                    await context.Vehicles.AnyAsync(v => v.Id == id, cancellation))
                .When(vehicle => vehicle.Id != Guid.Empty)
                .WithMessage("Vehicle not found with this id.");

        }
    }
}
