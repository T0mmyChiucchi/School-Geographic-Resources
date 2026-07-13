namespace SchoolGeoResources.Application.Places.Queries.GetPlaces;

using System;

public class PlaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string FullAddress { get; set; } = string.Empty;
}
