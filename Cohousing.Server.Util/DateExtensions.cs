using System;
using System.Globalization;

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
        
        public static int GetIso8601WeekNo(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        } 
    }
}