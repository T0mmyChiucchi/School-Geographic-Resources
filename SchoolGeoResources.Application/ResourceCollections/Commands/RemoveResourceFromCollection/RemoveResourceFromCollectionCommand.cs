namespace SchoolGeoResources.Application.ResourceCollections.Commands.RemoveResourceFromCollection;

using MediatR;
using System;

public class RemoveResourceFromCollectionCommand : IRequest<Unit>
{
    public Guid CollectionId { get; set; }
    public Guid ResourceId { get; set; }
}
