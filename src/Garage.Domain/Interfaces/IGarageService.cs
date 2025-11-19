using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garage.Domain.Models;

namespace Garage.Domain.Interfaces
{
    public interface IGarageService
    {
        Task Create(GarageModel garage);
        Task Update(GarageModel garage);
        Task Delete(Guid id);
        Task<GarageModel> GetById(Guid id);
        Task<IEnumerable<GarageModel>> GetAll();
    }
}
