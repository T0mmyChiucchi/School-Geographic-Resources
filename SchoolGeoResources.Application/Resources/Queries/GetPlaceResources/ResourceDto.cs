namespace SchoolGeoResources.Application.Resources.Queries.GetPlaceResources;

using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;

public class ResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public PublicationState State { get; set; }
    public string? MediaUrl { get; set; }
    public Guid PlaceId { get; set; }
}
