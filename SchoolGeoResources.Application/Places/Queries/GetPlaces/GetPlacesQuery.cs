namespace SchoolGeoResources.Application.Places.Queries.GetPlaces;

using MediatR;
using SchoolGeoResources.Application.Common.Models;

public class GetPlacesQuery : IRequest<PaginatedList<PlaceDto>>
{
    public double MinLat { get; set; }
    public double MaxLat { get; set; }
    public double MinLng { get; set; }
    public double MaxLng { get; set; }

    public string? SearchTerm { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
