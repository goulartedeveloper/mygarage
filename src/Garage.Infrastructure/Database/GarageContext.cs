using Garage.Infrastructure.Entities;
using Garage.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Garage.Infrastructure.Database;

public class GarageContext : DbContext
{
    public GarageContext(DbContextOptions<GarageContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VehicleMapping());
    }
}
