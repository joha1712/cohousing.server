using System;

namespace Cohousing.Server.Util
{
    public static class DateExtensions
    {
        public static DateTime StartOfWeekDate(this DateTime src)
        {
            var dayOfWeek = src.DayOfWeek;
            var offset = dayOfWeek != DayOfWeek.Sunday 
                ? dayOfWeek - DayOfWeek.Monday
                : 6;

            return src.Date.AddDays(-offset);
        }
    }
}