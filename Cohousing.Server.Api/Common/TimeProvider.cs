using System;
using Cohousing.Server.Model.Common;

namespace Cohousing.Server.Api.Common
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime ToLocal(DateTime utc)
        {
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);
        }

        public static DateTime FromLocal(DateTime localDateTime)
        {
            if (localDateTime.Kind == DateTimeKind.Utc)
            {
                return localDateTime;
            }

            return localDateTime.ToUniversalTime();
        }
    }
}