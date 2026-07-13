namespace SchoolGeoResources.Application.Places.Commands.CreatePlace;

using MediatR;
using System;

public class CreatePlaceCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}
