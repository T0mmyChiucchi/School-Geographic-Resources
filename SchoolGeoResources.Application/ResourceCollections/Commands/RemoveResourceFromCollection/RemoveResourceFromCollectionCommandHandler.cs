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

    public RemoveResourceFromCollectionCommandHandler(IRepository<ResourceCollection> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveResourceFromCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.CollectionId, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

        collection.RemoveResource(request.ResourceId);

        await _repository.UpdateAsync(collection, cancellationToken);

        return Unit.Value;
    }
}
