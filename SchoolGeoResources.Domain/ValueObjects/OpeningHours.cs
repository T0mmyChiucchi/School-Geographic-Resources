namespace SchoolGeoResources.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using SchoolGeoResources.Domain.Common;

public class OpeningHours : ValueObject
{
    public DayOfWeek DayOfWeek { get; }
    public TimeSpan OpenTime { get; }
    public TimeSpan CloseTime { get; }
    public string TimeZoneId { get; }

    private OpeningHours(DayOfWeek dayOfWeek, TimeSpan openTime, TimeSpan closeTime, string timeZoneId)
    {
        DayOfWeek = dayOfWeek;
        OpenTime = openTime;
        CloseTime = closeTime;
        TimeZoneId = timeZoneId;
    }

    public static OpeningHours Create(DayOfWeek dayOfWeek, TimeSpan openTime, TimeSpan closeTime, string timeZoneId)
    {
        if (openTime >= closeTime)
            throw new ArgumentException("OpenTime must be before CloseTime.");

        try
        {
            // Validate IANA timezone
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new ArgumentException("TimeZoneId must be a valid IANA timezone.", nameof(timeZoneId));
        }

        return new OpeningHours(dayOfWeek, openTime, closeTime, timeZoneId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DayOfWeek;
        yield return OpenTime;
        yield return CloseTime;
        yield return TimeZoneId;
    }
}
