using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolGeoResources.Application.Common.Interfaces;

public interface IUserContextService
{
    Task<OrganizationUser> GetCurrentUserAsync(CancellationToken ct);
    Task EnsureUserCanModifyOrganizationAsync(Guid organizationId, CancellationToken ct);
}
