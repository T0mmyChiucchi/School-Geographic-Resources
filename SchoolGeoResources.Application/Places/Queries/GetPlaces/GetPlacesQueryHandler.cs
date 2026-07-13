namespace SchoolGeoResources.Application.Places.Queries.GetPlaces;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetPlacesQueryHandler : IRequestHandler<GetPlacesQuery, List<PlaceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlacesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlaceDto>> Handle(GetPlacesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Places
            .Where(p => p.Location.Latitude >= request.MinLat && p.Location.Latitude <= request.MaxLat &&
                        p.Location.Longitude >= request.MinLng && p.Location.Longitude <= request.MaxLng)
            .Select(p => new PlaceDto
            {
                Id = p.Id,
                Name = p.Name,
                Latitude = p.Location.Latitude,
                Longitude = p.Location.Longitude,
                FullAddress = $"{p.Address.Street}, {p.Address.PostalCode} {p.Address.City}, {p.Address.CountryCode}"
            })
            .ToListAsync(cancellationToken);
    }
}
