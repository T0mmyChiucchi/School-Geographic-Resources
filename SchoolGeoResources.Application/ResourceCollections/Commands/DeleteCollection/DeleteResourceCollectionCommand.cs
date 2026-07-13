namespace SchoolGeoResources.Application.ResourceCollections.Commands.DeleteCollection;

using MediatR;
using System;

public class DeleteResourceCollectionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
