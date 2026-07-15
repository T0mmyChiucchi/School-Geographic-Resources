namespace SchoolGeoResources.Application.Places.Commands.UpdatePlace;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> _repository;
    private readonly IUserContextService _userContextService;

    public UpdatePlaceCommandHandler(
        IApplicationDbContext context,
        IRepository<SchoolGeoResources.Domain.Aggregates.PlaceAggregate.Place> repository,
        IUserContextService userContextService)
    {
        _context = context;
        _repository = repository;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (place == null)
            throw new Exception("Place not found");

        await _userContextService.EnsureUserCanModifyOrganizationAsync(place.OrganizationId, cancellationToken);

        var location = GeoCoordinate.Create(request.Latitude, request.Longitude);
        var address = Address.Create(request.Street, request.City, request.PostalCode, request.CountryCode);

        place.Update(request.Name, location);
        place.SetAddress(address);

        await _repository.UpdateAsync(place, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
