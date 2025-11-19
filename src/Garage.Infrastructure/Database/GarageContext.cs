using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ET = Garage.Infrastructure.Entities;
using Garage.Infrastructure.Interfaces;
using Garage.Infrastructure.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Garage.Infrastructure.Database;

public class GarageContext : IdentityDbContext<ET.ApplicationUser>
{
    private string _userId;

    public GarageContext(DbContextOptions<GarageContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _userId = currentUser.UserId;
    }

    public DbSet<ET.Vehicle> Vehicles { get; set; }
    public DbSet<ET.Garage> Garages { get; set; }

    public void SetUserId(string userId) => _userId ??= userId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VehicleMapping());
        modelBuilder.ApplyConfiguration(new GarageMapping());
        base.OnModelCreating(modelBuilder);

        if (!string.IsNullOrEmpty(_userId))
        {
            modelBuilder.Entity<ET.Vehicle>().HasQueryFilter(v => v.UserId == _userId);
            modelBuilder.Entity<ET.Garage>().HasQueryFilter(g => g.UserId == _userId);
        }

        modelBuilder.HasDefaultSchema("Garage");
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ET.Base || e.Entity is ET.UserBase);

        foreach (var entry in entries)
        {
            if (entry.Entity is ET.Base baseEntity && entry.State == EntityState.Modified)
                baseEntity.UpdatedAt = DateTime.UtcNow;

            if (entry.Entity is ET.UserBase userBaseEntity && entry.State == EntityState.Added)
                if (string.IsNullOrEmpty(userBaseEntity.UserId))
                    userBaseEntity.UserId = _userId;
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ET.Base || e.Entity is ET.UserBase);

        foreach (var entry in entries)
        {
            if (entry.Entity is ET.Base baseEntity && entry.State == EntityState.Modified)
                baseEntity.UpdatedAt = DateTime.UtcNow;

            if (entry.Entity is ET.UserBase userBaseEntity && entry.State == EntityState.Added)
                if (string.IsNullOrEmpty(userBaseEntity.UserId))
                    userBaseEntity.UserId = _userId;
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ET.Base || e.Entity is ET.UserBase);

        foreach (var entry in entries)
        {
            if (entry.Entity is ET.Base baseEntity && entry.State == EntityState.Modified)
                baseEntity.UpdatedAt = DateTime.UtcNow;

            if (entry.Entity is ET.UserBase userBaseEntity && entry.State == EntityState.Added)
                if (string.IsNullOrEmpty(userBaseEntity.UserId))
                    userBaseEntity.UserId = _userId;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ET.Base || e.Entity is ET.UserBase);

        foreach (var entry in entries)
        {
            if (entry.Entity is ET.Base baseEntity && entry.State == EntityState.Modified)
                baseEntity.UpdatedAt = DateTime.UtcNow;

            if (entry.Entity is ET.UserBase userBaseEntity && entry.State == EntityState.Added)
                if (string.IsNullOrEmpty(userBaseEntity.UserId))
                    userBaseEntity.UserId = _userId;
        }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
}
