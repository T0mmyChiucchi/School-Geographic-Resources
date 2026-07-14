namespace SchoolGeoResources.Application.Resources.Queries.SearchResources;

using MediatR;
using SchoolGeoResources.Application.Common.Models;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using System;

public class SearchResourcesQuery : IRequest<PaginatedList<ResourceSearchResultDto>>
{
    public string? SearchTerm { get; set; }
    public ResourceType? Type { get; set; }
    public string? Tag { get; set; }
    public Guid? PlaceId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
