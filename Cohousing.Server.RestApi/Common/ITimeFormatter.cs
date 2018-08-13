using System;

namespace Cohousing.Server.RestApi.Common
{
    public interface ITimeFormatter
    {
        string GetDayName(DateTime date);
        string GetDateName(DateTime date);
    }
}