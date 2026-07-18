namespace SchoolGeoResources.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using System.Linq;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.State)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(p => p.Location, loc =>
        {
            loc.Property(l => l.Latitude).HasColumnName("Latitude");
            loc.Property(l => l.Longitude).HasColumnName("Longitude");
            // Alternatively, map this to a PostGIS Geometry/Geography column
        });

        builder.OwnsOne(p => p.Address, addr =>
        {
            addr.Property(a => a.Street).HasMaxLength(200);
            addr.Property(a => a.City).HasMaxLength(100);
            addr.Property(a => a.PostalCode).HasMaxLength(20);
            addr.Property(a => a.CountryCode).HasMaxLength(2).IsFixedLength();
        });
    }
}
