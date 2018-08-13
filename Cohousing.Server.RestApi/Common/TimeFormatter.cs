using System;

namespace Cohousing.Server.RestApi.Common
{
    public class TimeFormatter : ITimeFormatter
    {
        private readonly ITimeProvider _timeProvider;

        public TimeFormatter(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public string GetDateName(DateTime date)
        {
            if (date.Kind != DateTimeKind.Local)
                date = _timeProvider.ToLocal(date);

            return date.ToString("d. MMMM");
        }

        public string GetDayName(DateTime date)
        {
            if (date.Kind != DateTimeKind.Local)
                date = _timeProvider.ToLocal(date);

            return date.ToString("dddd");
        }
    }
}