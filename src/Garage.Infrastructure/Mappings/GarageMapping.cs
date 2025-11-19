using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ET = Garage.Infrastructure.Entities;

namespace Garage.Infrastructure.Mappings
{
    public class GarageMapping : IEntityTypeConfiguration<ET.Garage>
    {
        public void Configure(EntityTypeBuilder<ET.Garage> builder)
        {
            builder.ToTable("Garages");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(g => g.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(g => g.UserId)
                .IsRequired()
                .HasMaxLength(36);
        }
    }
}
