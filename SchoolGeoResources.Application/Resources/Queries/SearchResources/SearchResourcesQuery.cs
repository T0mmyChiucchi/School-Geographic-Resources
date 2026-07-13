namespace SchoolGeoResources.Application.Resources.Queries.SearchResources;

using MediatR;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using System;
using System.Collections.Generic;

public class SearchResourcesQuery : IRequest<List<ResourceSearchResultDto>>
{
    public string? SearchTerm { get; set; }
    public ResourceType? Type { get; set; }
    public string? Tag { get; set; }
    public Guid? PlaceId { get; set; }
}
