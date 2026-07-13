namespace SchoolGeoResources.Application.ResourceCollections.Commands.UpdateCollection;

using MediatR;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.ValueObjects;
using System;
using System.Collections.Generic;

public class UpdateResourceCollectionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Visibility Visibility { get; set; }
    public List<string> Tags { get; set; } = new();
}
