using System;
using FluentValidation;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Garage.Domain.Validators
{
    public class GarageValidator : AbstractValidator<GarageModel>
    {
        public GarageValidator(GarageContext context)
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(200)
                .MinimumLength(4)
                .WithMessage("Name should have between 4 and 200 characters.");

            RuleFor(g => g.Id)
                .MustAsync(async (id, cancellation) =>
                    await context.Garages.AnyAsync(v => v.Id == id, cancellation))
                .When(vehicle => vehicle.Id != Guid.Empty)
                .WithMessage("Garage not found with this id.");
        }
    }
}
