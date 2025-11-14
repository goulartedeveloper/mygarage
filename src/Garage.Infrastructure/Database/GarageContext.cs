using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Garage.Infrastructure.Entities;
using Garage.Infrastructure.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage.Infrastructure.Database;

public class GarageContext : IdentityDbContext<ApplicationUser>
{
    public GarageContext(DbContextOptions<GarageContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VehicleMapping());
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Base && e.State == EntityState.Modified);

        foreach (var entry in entries)
            if (entry.Entity is Base baseEntity)
                baseEntity.UpdatedAt = DateTime.UtcNow;

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Base && e.State == EntityState.Modified);

        foreach (var entry in entries)
            if (entry.Entity is Base baseEntity)
                baseEntity.UpdatedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Base && e.State == EntityState.Modified);

        foreach (var entry in entries)
            if (entry.Entity is Base baseEntity)
                baseEntity.UpdatedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Base && e.State == EntityState.Modified);

        foreach (var entry in entries)
            if (entry.Entity is Base baseEntity)
                baseEntity.UpdatedAt = DateTime.UtcNow;

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
}
