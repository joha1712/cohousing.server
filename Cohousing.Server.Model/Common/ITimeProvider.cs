using System;

namespace Cohousing.Server.Model.Common
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime ToLocal(DateTime utc);
    }
}