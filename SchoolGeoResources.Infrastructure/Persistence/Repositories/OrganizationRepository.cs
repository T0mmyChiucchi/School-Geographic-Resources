namespace SchoolGeoResources.Infrastructure.Persistence.Repositories;

using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;

public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
