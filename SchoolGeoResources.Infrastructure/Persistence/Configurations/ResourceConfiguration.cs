namespace SchoolGeoResources.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System.Linq;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.MediaUrl)
            .HasMaxLength(2048);

        builder.OwnsOne(r => r.FileMetadata, fm =>
        {
            fm.Property(f => f.MimeType).HasMaxLength(100).HasColumnName("FileMimeType");
            fm.Property(f => f.SizeInBytes).HasColumnName("FileSizeInBytes");
        });

        // Store tags as a JSON array or a comma-separated string
        builder.Property(r => r.Tags)
            .HasConversion(
                tagSet => tagSet == null ? null : string.Join(",", tagSet.Tags),
                str => string.IsNullOrEmpty(str) ? TagSet.Create(new string[0]) : TagSet.Create(str.Split(',', System.StringSplitOptions.RemoveEmptyEntries))
            )
            .HasColumnName("Tags");
    }
}
