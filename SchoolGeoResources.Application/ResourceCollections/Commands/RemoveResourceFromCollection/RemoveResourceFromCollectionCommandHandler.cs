namespace SchoolGeoResources.Application.ResourceCollections.Commands.RemoveResourceFromCollection;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

public class RemoveResourceFromCollectionCommandHandler : IRequestHandler<RemoveResourceFromCollectionCommand, Unit>
{
    private readonly IRepository<ResourceCollection> _repository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public RemoveResourceFromCollectionCommandHandler(
        IRepository<ResourceCollection> repository,
        IApplicationDbContext context,
        IUserContextService userContextService)
    {
        _repository = repository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(RemoveResourceFromCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.CollectionId, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

        var place = await _context.Places.FindAsync(new object[] { collection.PlaceId }, cancellationToken);
        if (place != null)
        {
            await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);
        }

        collection.RemoveResource(request.ResourceId);

        await _repository.UpdateAsync(collection, cancellationToken);

        return Unit.Value;
    }
}
