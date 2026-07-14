namespace SchoolGeoResources.Application.ResourceCollections.Queries.GetPlaceCollections;

using MediatR;
using SchoolGeoResources.Application.Common.Models;
using System;

public class GetPlaceCollectionsQuery : IRequest<PaginatedList<ResourceCollectionDto>>
{
    public Guid PlaceId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
