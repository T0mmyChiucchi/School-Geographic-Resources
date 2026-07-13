namespace SchoolGeoResources.Application.Resources.Queries.SearchResources;

using System;
using System.Collections.Generic;

public class ResourceSearchResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();

    // Context info about the Place
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
}
