using System;

namespace Cohousing.Server.Api.Common
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime ToLocal(DateTime utc);
    }
}