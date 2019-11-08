using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.Startup
{
    public interface ICommonMealSettings
    {
        int NumberOfChefs { get; }
        int DefaultDaysToLoad { get; }
        IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> DefaultDinnerDates { get; }
    }
}