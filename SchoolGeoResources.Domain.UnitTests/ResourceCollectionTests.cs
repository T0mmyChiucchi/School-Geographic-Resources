using System;
using System.Linq;
using FluentAssertions;
using SchoolGeoResources.Domain.Aggregates.ResourceCollectionAggregate;
using SchoolGeoResources.Domain.Common;
using SchoolGeoResources.Domain.ValueObjects;
using Xunit;

namespace SchoolGeoResources.Domain.UnitTests.Aggregates.ResourceCollectionAggregate;

public class ResourceCollectionTests
{
    [Fact]
    public void Create_ShouldInitializeCorrectly()
    {
        var id = Guid.NewGuid();
        var placeId = Guid.NewGuid();
        var collection = ResourceCollection.Create(id, "Test Collection", placeId, Visibility.Public);

        collection.Id.Should().Be(id);
        collection.Title.Should().Be("Test Collection");
        collection.PlaceId.Should().Be(placeId);
        collection.Visibility.Should().Be(Visibility.Public);
        collection.IsArchived.Should().BeFalse();
        collection.Items.Should().BeEmpty();
    }

    [Fact]
    public void AddResource_WhenArchived_ShouldThrowInvalidOperationException()
    {
        var collection = ResourceCollection.Create(Guid.NewGuid(), "Test", Guid.NewGuid(), Visibility.Public);
        collection.Archive();

        Action act = () => collection.AddResource(Guid.NewGuid(), Visibility.Public);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cannot add resources to an archived collection.");
    }

    [Fact]
    public void AddResource_WithMorePermissiveVisibility_ShouldThrowInvalidOperationException()
    {
        var collection = ResourceCollection.Create(Guid.NewGuid(), "Private Collection", Guid.NewGuid(), Visibility.Private);

        Action act = () => collection.AddResource(Guid.NewGuid(), Visibility.Public);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Resource visibility cannot be more permissive than the collection's visibility.");
    }

    [Fact]
    public void AddResource_WithValidData_ShouldAddItem()
    {
        var collection = ResourceCollection.Create(Guid.NewGuid(), "Public Collection", Guid.NewGuid(), Visibility.Public);
        var resourceId = Guid.NewGuid();

        collection.AddResource(resourceId, Visibility.Private);

        collection.Items.Should().HaveCount(1);
        collection.Items.Single().ResourceId.Should().Be(resourceId);
        collection.Items.Single().Order.Should().Be(0);
    }
}
