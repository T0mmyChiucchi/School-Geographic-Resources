namespace SchoolGeoResources.Application.Resources.Queries.GetPlaceResources;

using MediatR;
using SchoolGeoResources.Application.Common.Models;
using System;

public class GetPlaceResourcesQuery : IRequest<PaginatedList<ResourceDto>>
{
    public Guid PlaceId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
