using System;

namespace Cohousing.WebSite.RestApi.Common
{
    public interface ITimeFormatter
    {
        string GetDayName(DateTime date);
        string GetDateName(DateTime date);
    }
}