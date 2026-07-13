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

    public DeleteResourceCollectionCommandHandler(IRepository<ResourceCollection> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteResourceCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

        collection.Archive();

        await _repository.UpdateAsync(collection, cancellationToken);

        return Unit.Value;
    }
}
