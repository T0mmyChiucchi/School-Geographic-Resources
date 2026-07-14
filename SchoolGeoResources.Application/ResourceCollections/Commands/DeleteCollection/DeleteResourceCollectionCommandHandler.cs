namespace SchoolGeoResources.Application.ResourceCollections.Commands.DeleteCollection;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

public class DeleteResourceCollectionCommandHandler : IRequestHandler<DeleteResourceCollectionCommand, Unit>
{
    private readonly IRepository<ResourceCollection> _repository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public DeleteResourceCollectionCommandHandler(
        IRepository<ResourceCollection> repository,
        IApplicationDbContext context,
        IUserContextService userContextService)
    {
        _repository = repository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(DeleteResourceCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

        var place = await _context.Places.FindAsync(new object[] { collection.PlaceId }, cancellationToken);
        if (place != null)
        {
            await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);
        }

        collection.Archive();

        await _repository.UpdateAsync(collection, cancellationToken);

        return Unit.Value;
    }
}
