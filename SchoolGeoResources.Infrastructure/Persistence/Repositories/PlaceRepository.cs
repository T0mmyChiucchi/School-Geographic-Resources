namespace SchoolGeoResources.Infrastructure.Persistence.Repositories;

using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;

public class PlaceRepository : Repository<Place>, IPlaceRepository
{
    public PlaceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
