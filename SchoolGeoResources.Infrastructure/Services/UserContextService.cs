using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.OrganizationAggregate;
using SchoolGeoResources.Domain.Aggregates.OrganizationUserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolGeoResources.Infrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public UserContextService(ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<OrganizationUser> GetCurrentUserAsync(CancellationToken ct)
    {
        var userIdStr = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or token is invalid.");
        }

        var user = await _context.OrganizationUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user == null)
        {
            // AUTO-PROVISION FOR DEVELOPMENT
            var org = await _context.Organizations.FirstOrDefaultAsync(ct);
            if (org == null) 
            {
                throw new UnauthorizedAccessException("User profile not found in the database and no organization available.");
            }
            
            user = OrganizationUser.Create(userId, "auto-provisioned@example.com", "Auto", "User", org.Id, OrganizationRole.OrgAdmin);
            _context.OrganizationUsers.Add(user);
            await _context.SaveChangesAsync(ct);
        }

        return user;
    }

    public async Task EnsureUserCanModifyOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        var user = await GetCurrentUserAsync(ct);
        if (user.OrganizationId != organizationId)
        {
            throw new UnauthorizedAccessException("User does not have permission to modify resources in this organization.");
        }
    }
}
