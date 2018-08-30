using System;

namespace Cohousing.Server.Api.Startup
{
    public interface ICommonMealSettings
    {
        TimeSpan DefaultMealTime { get; }
        int DefaultNumberOfChefs { get; }
        int DefaultDaysShown { get; }
    }
}