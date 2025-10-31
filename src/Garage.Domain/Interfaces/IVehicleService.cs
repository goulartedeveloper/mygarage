using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garage.Domain.Models;

namespace Garage.Domain.Interfaces
{
    public interface IVehicleService
    {
        Task Create(VehicleModel vehicle);
        Task Update(VehicleModel vehicle);
        Task Delete(Guid id);
        Task<VehicleModel> GetById(Guid id);
        Task<IEnumerable<VehicleModel>> GetAll();
    }
}