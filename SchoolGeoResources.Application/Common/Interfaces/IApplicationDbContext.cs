namespace SchoolGeoResources.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;

public interface IApplicationDbContext
{
    DbSet<Place> Places { get; }
    DbSet<Resource> Resources { get; }
    DbSet<Organization> Organizations { get; }
    DbSet<OrganizationUser> OrganizationUsers { get; }
    DbSet<ResourceCollection> ResourceCollections { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
