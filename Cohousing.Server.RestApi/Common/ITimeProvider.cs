using System;

namespace Cohousing.WebSite.RestApi.Common
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime ToLocal(DateTime utc);
    }
}