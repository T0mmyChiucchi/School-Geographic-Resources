namespace SchoolGeoResources.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using SchoolGeoResources.Domain.ValueObjects;

public class ResourceCollectionConfiguration : IEntityTypeConfiguration<ResourceCollection>
{
    public void Configure(EntityTypeBuilder<ResourceCollection> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rc => rc.Tags)
            .HasConversion(
                tagSet => tagSet == null ? null : string.Join(",", tagSet.Tags),
                str => string.IsNullOrEmpty(str) ? TagSet.Create(new string[0]) : TagSet.Create(str.Split(',', System.StringSplitOptions.RemoveEmptyEntries))
            )
            .HasColumnName("Tags");

        builder.HasMany(rc => rc.Items)
            .WithOne()
            .HasForeignKey("ResourceCollectionId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(rc => rc.Items).AutoInclude();
    }
}
