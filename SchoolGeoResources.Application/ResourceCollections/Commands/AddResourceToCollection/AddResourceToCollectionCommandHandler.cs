namespace SchoolGeoResources.Application.ResourceCollections.Commands.AddResourceToCollection;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

public class AddResourceToCollectionCommandHandler : IRequestHandler<AddResourceToCollectionCommand>
{
    private readonly IResourceCollectionRepository _collectionRepository;
    private readonly IResourceRepository _resourceRepository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public AddResourceToCollectionCommandHandler(
        IResourceCollectionRepository collectionRepository,
        IResourceRepository resourceRepository,
        IApplicationDbContext context,
        IUserContextService userContextService)
    {
        _collectionRepository = collectionRepository;
        _resourceRepository = resourceRepository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task Handle(AddResourceToCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetByIdAsync(request.CollectionId);
        if (collection == null)
        {
            throw new ArgumentException($"Collection with ID {request.CollectionId} not found.");
        }

        var place = await _context.Places.FindAsync(new object[] { collection.PlaceId }, cancellationToken);
        if (place != null)
        {
            await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);
        }

        var resource = await _resourceRepository.GetByIdAsync(request.ResourceId);
        if (resource == null)
        {
            throw new ArgumentException($"Resource with ID {request.ResourceId} not found.");
        }

        // AddResource enforces domain rules like Visibility
        collection.AddResource(resource.Id, resource.Visibility);

        await _collectionRepository.UpdateAsync(collection);
    }
}
