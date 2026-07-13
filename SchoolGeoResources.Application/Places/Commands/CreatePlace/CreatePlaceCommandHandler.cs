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
    private readonly ICurrentUserService _currentUserService;

    public CreatePlaceCommandHandler(IPlaceRepository placeRepository, IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _placeRepository = placeRepository;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user id.");
        }

        var orgUser = await _context.OrganizationUsers.FindAsync(new object[] { userId }, cancellationToken);
        if (orgUser == null)
        {
            // For now, allow creation without a valid organization for testing if not found, or throw.
            // Let's assume they MUST have an organization.
            throw new UnauthorizedAccessException("User does not belong to any organization.");
        }

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
