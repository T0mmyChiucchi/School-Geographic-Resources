namespace SchoolGeoResources.Application.Places.Commands.CreatePlace;

using MediatR;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Domain.Aggregates.PlaceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, Guid>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IApplicationDbContext _context;
    private readonly IUserContextService _userContextService;

    public CreatePlaceCommandHandler(IPlaceRepository placeRepository, IApplicationDbContext context, IUserContextService userContextService)
    {
        _placeRepository = placeRepository;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<Guid> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
    {
        var orgUser = await _userContextService.GetCurrentUserAsync(cancellationToken);

        var place = Place.Create(
            Guid.NewGuid(), 
            request.Name, 
            GeoCoordinate.Create(request.Latitude, request.Longitude), 
            orgUser.OrganizationId);

        var address = Address.Create(request.Street, request.City, request.PostalCode, request.CountryCode);
        place.SetAddress(address);

        await _placeRepository.AddAsync(place, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return place.Id;
    }
}
