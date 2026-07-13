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

    public UpdateResourceCollectionCommandHandler(IRepository<ResourceCollection> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateResourceCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (collection == null)
            throw new Exception("ResourceCollection not found");

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
