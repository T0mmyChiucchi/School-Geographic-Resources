namespace SchoolGeoResources.Application.ResourceCollections.Queries.GetPlaceCollections;

using System;
using System.Collections.Generic;

public class ResourceCollectionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid PlaceId { get; set; }
    public string Visibility { get; set; } = string.Empty;
    public bool IsArchived { get; set; }

    public List<CollectionItemDto> Items { get; set; } = new();
}

public class CollectionItemDto
{
    public Guid ResourceId { get; set; }
    public int Order { get; set; }
}
