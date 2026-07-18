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
    public PublicationState State { get; private set; } = PublicationState.Draft;

    private Place() { } // EF Core

    private Place(Guid id, string name, GeoCoordinate location, Guid organizationId) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        OrganizationId = organizationId;
        State = PublicationState.Draft;
    }

    public static Place Create(Guid id, string name, GeoCoordinate location, Guid organizationId)
    {
        return new Place(id, name, location, organizationId);
    }

    public void ChangeState(PublicationState newState)
    {
        State = newState;
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }

    public void Update(string name, GeoCoordinate location)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Location = location ?? throw new ArgumentNullException(nameof(location));
    }
}
