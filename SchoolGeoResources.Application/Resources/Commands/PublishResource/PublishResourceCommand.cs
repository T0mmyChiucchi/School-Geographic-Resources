namespace SchoolGeoResources.Application.Resources.Commands.PublishResource;

using MediatR;
using System;

public class PublishResourceCommand : IRequest
{
    public Guid ResourceId { get; set; }
}
