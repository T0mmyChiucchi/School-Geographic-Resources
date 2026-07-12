namespace SchoolGeoResources.Domain.Aggregates.ResourceAggregate;

using System;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.ValueObjects;

public class Resource : AggregateRoot
{
    public string Title { get; private set; }
    public ResourceType Type { get; private set; }
    public PublicationState State { get; private set; }
    public Visibility Visibility { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid PlaceId { get; private set; }
    public string MediaUrl { get; private set; }
    public FileMetadata FileMetadata { get; private set; }
    public TagSet Tags { get; private set; }

    private Resource() { } // EF Core

    private Resource(Guid id, string title, ResourceType type, Guid organizationId, Guid placeId) : base(id)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Type = type;
        OrganizationId = organizationId;
        PlaceId = placeId;
        State = PublicationState.Draft;
        Visibility = Visibility.Private; // Default
    }

    public static Resource Create(Guid id, string title, ResourceType type, Guid organizationId, Guid placeId)
    {
        return new Resource(id, title, type, organizationId, placeId);
    }

    public void SetMediaUrl(string url)
    {
        if (State == PublicationState.Published)
            throw new InvalidOperationException("Cannot modify a published resource directly. Create a new version.");

        if (string.IsNullOrWhiteSpace(url))
        {
            MediaUrl = null;
            return;
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) || 
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("MediaUrl must be a valid HTTP or HTTPS URI.", nameof(url));
        }

        MediaUrl = url;
    }

    public void SetFileMetadata(FileMetadata metadata)
    {
        if (State == PublicationState.Published)
            throw new InvalidOperationException("Cannot modify a published resource directly.");

        if (Type != ResourceType.Document && metadata != null)
            throw new InvalidOperationException("FileMetadata can only be set for Document resource types.");

        FileMetadata = metadata;
    }

    public void SetVisibility(Visibility visibility)
    {
        if (State == PublicationState.Published)
            throw new InvalidOperationException("Cannot modify a published resource directly.");

        Visibility = visibility;
    }

    public void SubmitForReview()
    {
        if (State != PublicationState.Draft && State != PublicationState.Rejected)
            throw new InvalidOperationException($"Cannot transition from {State} to InReview.");

        if (Type == ResourceType.Document && FileMetadata == null)
            throw new InvalidOperationException("A Document resource must have FileMetadata before being submitted.");

        State = PublicationState.InReview;
    }

    public void Publish()
    {
        if (State != PublicationState.InReview)
            throw new InvalidOperationException($"Cannot transition from {State} to Published.");

        State = PublicationState.Published;
    }

    public void Reject()
    {
        if (State != PublicationState.InReview)
            throw new InvalidOperationException($"Cannot transition from {State} to Rejected.");

        State = PublicationState.Rejected;
    }

    public void Archive()
    {
        if (State != PublicationState.Published && State != PublicationState.Rejected)
             throw new InvalidOperationException($"Cannot transition from {State} to Archived.");

        State = PublicationState.Archived;
    }
}
