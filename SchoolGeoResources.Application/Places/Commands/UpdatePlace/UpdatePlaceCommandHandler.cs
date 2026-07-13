namespace SchoolGeoResources.Application.Places.Commands.UpdatePlace;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, Unit>
{
    private readonly IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> _repository;

    public UpdatePlaceCommandHandler(IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (place == null)
            throw new Exception("Place not found");

        var location = GeoCoordinate.Create(request.Latitude, request.Longitude);
        var address = Address.Create(request.Street, request.City, request.PostalCode, request.CountryCode);

        place.Update(request.Name, location);
        place.SetAddress(address);

        await _repository.UpdateAsync(place, cancellationToken);

        return Unit.Value;
    }
}
