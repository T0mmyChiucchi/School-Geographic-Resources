namespace SchoolGeoResources.Infrastructure.Persistence.Repositories;

using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;

public class ResourceRepository : Repository<Resource>, IResourceRepository
{
    public ResourceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
