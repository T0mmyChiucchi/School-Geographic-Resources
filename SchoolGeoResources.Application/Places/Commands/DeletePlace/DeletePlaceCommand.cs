namespace SchoolGeoResources.Application.Places.Commands.DeletePlace;

using MediatR;
using System;

public class DeletePlaceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
