namespace SchoolGeoResources.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using SchoolGeoResources.Domain.Common;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string CountryCode { get; }

    private Address(string street, string city, string postalCode, string countryCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
        CountryCode = countryCode;
    }

    public static Address Create(string street, string city, string postalCode, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            throw new ArgumentException("CountryCode must be a valid 2-character ISO code.", nameof(countryCode));

        return new Address(street, city, postalCode, countryCode.ToUpperInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
        yield return CountryCode;
    }
}
