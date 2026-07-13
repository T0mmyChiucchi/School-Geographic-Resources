namespace SchoolGeoResources.Application.Resources.Commands.UpdateResource;

using MediatR;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Collections.Generic;

public class UpdateResourceCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public Visibility Visibility { get; set; }
    public string? MediaUrl { get; set; }
    public List<string> Tags { get; set; } = new();
}
