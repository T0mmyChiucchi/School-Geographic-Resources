namespace SchoolGeoResources.Application.Resources.Commands.PublishResource;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishResourceCommandHandler : IRequestHandler<PublishResourceCommand>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public PublishResourceCommandHandler(IResourceRepository resourceRepository, IApplicationDbContext context, IUserContextService userContextService)
    {
        _resourceRepository = resourceRepository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task Handle(PublishResourceCommand request, CancellationToken cancellationToken)
    {
        var orgUser = await _userContextService.GetCurrentUserAsync(cancellationToken);

        var resource = await _resourceRepository.GetByIdAsync(request.ResourceId, cancellationToken);
        if (resource == null)
        {
            throw new ArgumentException("Resource not found.");
        }

        // Verify the user belongs to the organization that owns this resource
        if (resource.OrganizationId != orgUser.OrganizationId)
        {
            throw new UnauthorizedAccessException("You do not have permission to publish this resource.");
        }

        // Fast-track publish
        if (resource.State == PublicationState.Draft)
        {
            resource.SubmitForReview();
        }

        if (resource.State == PublicationState.InReview)
        {
            resource.Publish();
        }

        await _resourceRepository.UpdateAsync(resource, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
