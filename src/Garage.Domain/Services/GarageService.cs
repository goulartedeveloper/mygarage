using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garage.Domain.Interfaces;
using Garage.Domain.Models;
using Garage.Infrastructure.Database;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ET = Garage.Infrastructure.Entities;

namespace Garage.Domain.Services
{
    public class GarageService : IGarageService
    {
        private readonly GarageContext _context;

        public GarageService(GarageContext context)
        {
            _context = context;
        }

        public async Task Create(GarageModel garage)
        {
            var garageEntity = garage.Adapt<ET.Garage>();

            await _context.Garages.AddAsync(garageEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var garage = await _context.Garages.FindAsync(id);

            if (garage is not null)
            {
                _context.Garages.Remove(garage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GarageModel>> GetAll()
        {
            var garages = await _context.Garages.ToListAsync();

            return garages.Adapt<IEnumerable<GarageModel>>();
        }

        public async Task<GarageModel> GetById(Guid id)
        {
            var garage = await _context.Garages.FindAsync(id);
            return garage.Adapt<GarageModel>();
        }

        public async Task Update(GarageModel garage)
        {
            var garageEntity = await _context.Garages.FindAsync(garage.Id);

            if (garageEntity is not null)
            {
                garage.Adapt(garageEntity);

                _context.Garages.Update(garageEntity);

                await _context.SaveChangesAsync();
            }
        }
    }
}
