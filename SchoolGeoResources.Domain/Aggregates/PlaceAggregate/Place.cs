namespace SchoolGeoResources.Domain.Aggregates.PlaceAggregate;

using System;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.ValueObjects;

public class Place : AggregateRoot
{
    public string Name { get; private set; }
    public GeoCoordinate Location { get; private set; }
    public Address Address { get; private set; }
    public Guid OrganizationId { get; private set; }

    private Place() { } // EF Core

    private Place(Guid id, string name, GeoCoordinate location, Guid organizationId) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        OrganizationId = organizationId;
    }

    public static Place Create(Guid id, string name, GeoCoordinate location, Guid organizationId)
    {
        return new Place(id, name, location, organizationId);
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }
}
