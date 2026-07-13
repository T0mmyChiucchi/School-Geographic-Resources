namespace SchoolGeoResources.Application.ResourceCollections.Queries.GetPlaceCollections;

using MediatR;
using System;
using System.Collections.Generic;

public class GetPlaceCollectionsQuery : IRequest<List<ResourceCollectionDto>>
{
    public Guid PlaceId { get; set; }
}
