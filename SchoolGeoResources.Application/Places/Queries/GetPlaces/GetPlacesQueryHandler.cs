namespace SchoolGeoResources.Application.Places.Queries.GetPlaces;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Application.Common.Mappings;
using SchoolGeoResources.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetPlacesQueryHandler : IRequestHandler<GetPlacesQuery, PaginatedList<PlaceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPlacesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<PlaceDto>> Handle(GetPlacesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Places.AsQueryable();

        if (request.MinLat != 0 || request.MaxLat != 0 || request.MinLng != 0 || request.MaxLng != 0)
        {
            query = query.Where(p => 
                p.Location.Latitude >= request.MinLat &&
                p.Location.Latitude <= request.MaxLat &&
                p.Location.Longitude >= request.MinLng &&
                p.Location.Longitude <= request.MaxLng);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
        }

        var projection = query.Select(p => new PlaceDto
        {
            Id = p.Id,
            Name = p.Name,
            Latitude = p.Location.Latitude,
            Longitude = p.Location.Longitude,
            FullAddress = p.Address.City,
            Description = p.Address.Street,
            OrganizationId = p.OrganizationId
        });

        return await projection.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
