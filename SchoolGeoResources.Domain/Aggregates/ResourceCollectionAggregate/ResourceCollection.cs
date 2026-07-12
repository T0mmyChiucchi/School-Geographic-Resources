namespace SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;

using System;
using System.Collections.Generic;
using System.Linq;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.ValueObjects;

public class ResourceCollection : AggregateRoot
{
    public string Title { get; private set; }
    public Guid PlaceId { get; private set; }
    public Visibility Visibility { get; private set; }
    public TagSet Tags { get; private set; }
    public bool IsArchived { get; private set; }

    private readonly List<CollectionItem> _items = new();
    public IReadOnlyCollection<CollectionItem> Items => _items.AsReadOnly();

    private ResourceCollection() { } // EF Core

    private ResourceCollection(Guid id, string title, Guid placeId, Visibility visibility) : base(id)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        PlaceId = placeId;
        Visibility = visibility;
        IsArchived = false;
    }

    public static ResourceCollection Create(Guid id, string title, Guid placeId, Visibility visibility)
    {
        return new ResourceCollection(id, title, placeId, visibility);
    }

    public void SetTags(TagSet tags)
    {
        Tags = tags;
    }

    public void Archive()
    {
        IsArchived = true;
    }

    public void AddResource(Guid resourceId, Visibility resourceVisibility)
    {
        if (IsArchived)
            throw new InvalidOperationException("Cannot add resources to an archived collection.");

        // Invariant: La visibilità di una Resource non può essere più permissiva di quella della Collection padre.
        if (IsMorePermissive(resourceVisibility, Visibility))
            throw new InvalidOperationException("Resource visibility cannot be more permissive than the collection's visibility.");

        var newOrder = _items.Any() ? _items.Max(i => i.Order) + 1 : 0;
        
        _items.Add(new CollectionItem(resourceId, newOrder));
    }

    public void ReorderResource(Guid resourceId, int newOrder)
    {
        if (newOrder < 0) throw new ArgumentOutOfRangeException(nameof(newOrder));

        var item = _items.SingleOrDefault(i => i.ResourceId == resourceId);
        if (item == null) throw new InvalidOperationException("Resource is not in this collection.");

        // Simple swap logic or full reorder logic. 
        // For simplicity, we just enforce that Order must be unique eventually, 
        // but here we just update it. A proper implementation would shift other items.
        item.UpdateOrder(newOrder);
    }

    private bool IsMorePermissive(Visibility child, Visibility parent)
    {
        // Public > AuthenticatedOnly > Private
        if (parent == Visibility.Private && child != Visibility.Private) return true;
        if (parent == Visibility.AuthenticatedOnly && child == Visibility.Public) return true;
        return false;
    }
}

public class CollectionItem : Entity
{
    public Guid ResourceId { get; private set; }
    public int Order { get; private set; }

    internal CollectionItem(Guid resourceId, int order) : base(Guid.NewGuid())
    {
        ResourceId = resourceId;
        Order = order;
    }

    internal void UpdateOrder(int newOrder)
    {
        Order = newOrder;
    }
}
