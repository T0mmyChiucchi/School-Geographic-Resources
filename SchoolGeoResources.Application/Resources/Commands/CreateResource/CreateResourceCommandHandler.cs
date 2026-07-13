namespace SchoolGeoResources.Application.Resources.Commands.CreateResource;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, Guid>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateResourceCommandHandler(IResourceRepository resourceRepository, IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _resourceRepository = resourceRepository;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user id.");
        }

        var orgUser = await _context.OrganizationUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (orgUser == null)
        {
            throw new UnauthorizedAccessException("User does not belong to any organization.");
        }

        var placeExists = await _context.Places.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);
        if (!placeExists)
        {
            throw new ArgumentException("Place not found.");
        }

        var resource = Resource.Create(
            Guid.NewGuid(),
            request.Title,
            request.Type,
            orgUser.OrganizationId,
            request.PlaceId
        );

        if (request.Type == ResourceType.Document)
        {
            if (request.FileName != null && request.ContentType != null && request.SizeBytes.HasValue)
            {
                var fileMetadata = FileMetadata.Create(request.ContentType, request.SizeBytes.Value);
                resource.SetFileMetadata(fileMetadata);
            }
        }
        
        if (!string.IsNullOrEmpty(request.MediaUrl))
        {
            resource.SetMediaUrl(request.MediaUrl);
        }

        resource.SetVisibility(request.Visibility);

        await _resourceRepository.AddAsync(resource, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return resource.Id;
    }
}
