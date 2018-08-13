using System;

namespace Cohousing.Server.RestApi.Common
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime ToLocal(DateTime utc);
    }
}