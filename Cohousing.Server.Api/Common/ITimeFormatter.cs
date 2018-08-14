using System;

namespace Cohousing.Server.Api.Common
{
    public interface ITimeFormatter
    {
        string GetDayName(DateTime date);
        string GetDateName(DateTime date);
    }
}