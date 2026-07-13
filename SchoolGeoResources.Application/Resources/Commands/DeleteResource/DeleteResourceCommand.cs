namespace SchoolGeoResources.Application.Resources.Commands.DeleteResource;

using MediatR;
using System;

public class DeleteResourceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
