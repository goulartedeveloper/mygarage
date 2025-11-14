using Garage.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Garage.Infrastructure.Mappings;

public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Make)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Year)
            .IsRequired();

        builder.Property(v => v.Plate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.Color)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(v => v.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(v => v.UserId)
            .IsRequired()
            .HasMaxLength(36);

        builder.HasIndex(v => new { v.Plate, v.UserId })
            .IsUnique();
    }
}
