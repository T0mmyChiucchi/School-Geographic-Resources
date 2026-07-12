namespace SchoolGeoResources.Domain.Events;

using System;
using SchoolGeoResources.Domain.Common;

public class PlaceRegisteredEvent : DomainEvent
{
    public Guid PlaceId { get; }
    public Guid OrganizationId { get; }

    public PlaceRegisteredEvent(Guid placeId, Guid organizationId)
    {
        PlaceId = placeId;
        OrganizationId = organizationId;
    }
}

public class PlacePublishedEvent : DomainEvent
{
    public Guid PlaceId { get; }

    public PlacePublishedEvent(Guid placeId)
    {
        PlaceId = placeId;
    }
}

public class OrganizationApprovedEvent : DomainEvent
{
    public Guid OrganizationId { get; }

    public OrganizationApprovedEvent(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}

public class ResourceCreatedEvent : DomainEvent
{
    public Guid ResourceId { get; }
    public Guid PlaceId { get; }

    public ResourceCreatedEvent(Guid resourceId, Guid placeId)
    {
        ResourceId = resourceId;
        PlaceId = placeId;
    }
}

public class ResourcePublishedEvent : DomainEvent
{
    public Guid ResourceId { get; }

    public ResourcePublishedEvent(Guid resourceId)
    {
        ResourceId = resourceId;
    }
}

public class ResourceRejectedEvent : DomainEvent
{
    public Guid ResourceId { get; }

    public ResourceRejectedEvent(Guid resourceId)
    {
        ResourceId = resourceId;
    }
}

public class VisitStartedEvent : DomainEvent
{
    public Guid PlaceId { get; }
    public Guid? UserId { get; } // Nullable for anonymous viewers

    public VisitStartedEvent(Guid placeId, Guid? userId)
    {
        PlaceId = placeId;
        UserId = userId;
    }
}

public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserRegisteredEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}
