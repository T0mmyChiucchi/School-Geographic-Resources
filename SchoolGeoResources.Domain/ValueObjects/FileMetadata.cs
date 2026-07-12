namespace SchoolGeoResources.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using SchoolGeoResources.Domain.Common;

public class FileMetadata : ValueObject
{
    public string MimeType { get; }
    public long SizeInBytes { get; }

    private const long MaxSizeBytes = 50 * 1024 * 1024; // 50MB

    private FileMetadata(string mimeType, long sizeInBytes)
    {
        MimeType = mimeType;
        SizeInBytes = sizeInBytes;
    }

    public static FileMetadata Create(string mimeType, long sizeInBytes)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
            throw new ArgumentException("MIME type cannot be empty.", nameof(mimeType));

        if (sizeInBytes <= 0 || sizeInBytes > MaxSizeBytes)
            throw new ArgumentOutOfRangeException(nameof(sizeInBytes), $"File size must be greater than 0 and less than or equal to 50MB.");

        return new FileMetadata(mimeType, sizeInBytes);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MimeType;
        yield return SizeInBytes;
    }
}
