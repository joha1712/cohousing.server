using System;

namespace Cohousing.Server.Api.Common
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTime ToLocal(DateTime utc)
        {
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);
        }
    }
}