using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garage.Domain.Interfaces;
using Garage.Domain.Messages.Vehicle;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Garage.Infrastructure.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Rebus.Bus;

namespace Garage.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly GarageContext _context;
        private readonly IBus _bus;

        public VehicleService(GarageContext context, IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        public async Task Create(VehicleModel vehicle)
        {
            var vehicleEntity = vehicle.Adapt<Vehicle>();

            var transaction = await _context.Database.BeginTransactionAsync();

            _context.Vehicles.Add(vehicleEntity);
            await _context.SaveChangesAsync();

            await _bus.Send(vehicle.Adapt<VehicleCreatedMessage>());

            await transaction.CommitAsync();
        }

        public async Task Delete(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle is not null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VehicleModel>> GetAll()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return vehicles.Adapt<IEnumerable<VehicleModel>>();
        }

        public async Task<VehicleModel> GetById(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            return vehicle.Adapt<VehicleModel>();
        }

        public async Task Update(VehicleModel vehicle)
        {
            var vehicleEntity = await _context.Vehicles
                .FirstOrDefaultAsync(x => x.Id == vehicle.Id);

            if (vehicleEntity is not null)
            {
                vehicle.Adapt(vehicleEntity);
                _context.Vehicles.Update(vehicleEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
