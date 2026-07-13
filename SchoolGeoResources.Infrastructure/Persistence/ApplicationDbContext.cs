namespace SchoolGeoResources.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;
using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using System.Reflection;

public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationUser> OrganizationUsers { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceCollection> ResourceCollections { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis"); // Enable PostGIS

        modelBuilder.Ignore<SchoolGeoResources.Domain.Common.DomainEvent>();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
