namespace SchoolGeoResources.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using SchoolGeoResources.Domain.Common;

public class TagSet : ValueObject
{
    private readonly HashSet<string> _tags;

    public IReadOnlySet<string> Tags => _tags;

    private TagSet() { } // EF Core

    private TagSet(HashSet<string> tags)
    {
        _tags = tags;
    }

    public static TagSet Create(IEnumerable<string> tags)
    {
        var tagList = tags?.ToList() ?? new List<string>();

        if (tagList.Count > 20)
            throw new ArgumentException("Maximum 20 tags allowed.");

        var validTags = new HashSet<string>();

        foreach (var tag in tagList)
        {
            if (string.IsNullOrWhiteSpace(tag))
                continue;

            if (tag.Length < 2 || tag.Length > 50)
                throw new ArgumentException($"Tag '{tag}' must be between 2 and 50 characters.");

            if (tag.Contains(" "))
                throw new ArgumentException($"Tag '{tag}' must not contain spaces.");

            validTags.Add(tag.ToLowerInvariant());
        }

        return new TagSet(validTags);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var tag in _tags.OrderBy(t => t))
        {
            yield return tag;
        }
    }
}
