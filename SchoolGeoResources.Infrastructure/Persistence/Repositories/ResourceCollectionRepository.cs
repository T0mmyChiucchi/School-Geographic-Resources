namespace SchoolGeoResources.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using System;
using System.Threading.Tasks;

public class ResourceCollectionRepository : Repository<ResourceCollection>, IResourceCollectionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ResourceCollectionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ResourceCollection> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ResourceCollections
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
