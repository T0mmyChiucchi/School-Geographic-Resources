namespace SchoolGeoResources.Application.Places.Commands.UpdatePlaceState;

using MediatR;
using SchoolGeoResources.Domain.ValueObjects;
using System;

public class UpdatePlaceStateCommand : IRequest
{
    public Guid Id { get; set; }
    public PublicationState NewState { get; set; }
}
