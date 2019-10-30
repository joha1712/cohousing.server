using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.Startup
{
    public interface ICommonMealSettings
    {
        int NumberOfChefs { get; }
        int MaxDinnerDaysShown { get; }
        IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> StandardDinnerDates { get; }
    }
}