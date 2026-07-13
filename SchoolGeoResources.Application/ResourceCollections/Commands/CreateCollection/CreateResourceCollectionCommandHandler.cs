namespace SchoolGeoResources.Application.ResourceCollections.Commands.CreateCollection;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateResourceCollectionCommandHandler : IRequestHandler<CreateResourceCollectionCommand, Guid>
{
    private readonly IResourceCollectionRepository _repository;
    private readonly IPlaceRepository _placeRepository;

    public CreateResourceCollectionCommandHandler(
        IResourceCollectionRepository repository,
        IPlaceRepository placeRepository)
    {
        _repository = repository;
        _placeRepository = placeRepository;
    }

    public async Task<Guid> Handle(CreateResourceCollectionCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId);
        if (place == null)
        {
            throw new ArgumentException($"Place with ID {request.PlaceId} does not exist.", nameof(request.PlaceId));
        }

        var collection = ResourceCollection.Create(Guid.NewGuid(), request.Title, request.PlaceId, request.Visibility);

        await _repository.AddAsync(collection);

        return collection.Id;
    }
}
