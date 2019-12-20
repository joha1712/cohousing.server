using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Service
{
    public interface ICommonMealStatisticsService
    {
         Task<IImmutableList<CommonMealStatisticsOverview>> LoadOverview(DateTime fromDate, DateTime toDate);
    }
}