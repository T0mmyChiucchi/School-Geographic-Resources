namespace SchoolGeoResources.Application.Resources.Queries.GetPlaceResources;

using MediatR;
using System;
using System.Collections.Generic;

public class GetPlaceResourcesQuery : IRequest<List<ResourceDto>>
{
    public Guid PlaceId { get; set; }
}
