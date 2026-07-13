namespace SchoolGeoResources.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;

public interface IApplicationDbContext
{
    DbSet<Place> Places { get; }
    DbSet<SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate.OrganizationUser> OrganizationUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
