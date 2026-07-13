namespace SchoolGeoResources.Application.Resources.Commands.CreateResource;

using MediatR;
using SchoolGeoResources.Domain.Aggregates.ResourceAggregate;
using SchoolGeoResources.Domain.ValueObjects;
using System;

public class CreateResourceCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public Guid PlaceId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public Visibility Visibility { get; set; } = Visibility.Private;
    
    // FileMetadata properties (for Documents)
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long? SizeBytes { get; set; }
}
