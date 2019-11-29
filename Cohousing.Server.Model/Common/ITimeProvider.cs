using System;

namespace Cohousing.Server.Model.Common
{
    public interface ITimeProvider
    {
        DateTime Now();
        DateTime ToLocal(DateTime utc);
    }
}