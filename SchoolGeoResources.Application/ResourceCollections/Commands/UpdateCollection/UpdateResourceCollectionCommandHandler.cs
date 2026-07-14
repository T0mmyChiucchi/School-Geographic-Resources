namespace SchoolGeoResources.Application.ResourceCollections.Commands.UpdateCollection;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateResourceCollectionCommandHandler : IRequestHandler<UpdateResourceCollectionCommand, Unit>
{
    private readonly IRepository<ResourceCollection> _repository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public UpdateResourceCollectionCommandHandler(
        IRepository<ResourceCollection> repository,
        IApplicationDbContext context,
        IUserContextService userContextService)
    {
        _repository = repository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(UpdateResourceCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

        var place = await _context.Places.FindAsync(new object[] { collection.PlaceId }, cancellationToken);
        if (place != null)
        {
            await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);
        }

        collection.UpdateTitle(request.Title);
        collection.SetVisibility(request.Visibility);

        if (request.Tags != null)
        {
            collection.SetTags(TagSet.Create(request.Tags));
        }

        await _repository.UpdateAsync(collection, cancellationToken);

        return Unit.Value;
    }
}
