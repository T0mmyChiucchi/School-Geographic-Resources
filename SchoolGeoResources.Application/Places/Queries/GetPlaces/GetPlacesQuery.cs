namespace SchoolGeoResources.Application.Places.Queries.GetPlaces;

using MediatR;
using System.Collections.Generic;

public class GetPlacesQuery : IRequest<List<PlaceDto>>
{
    public double MinLat { get; set; }
    public double MaxLat { get; set; }
    public double MinLng { get; set; }
    public double MaxLng { get; set; }
}
