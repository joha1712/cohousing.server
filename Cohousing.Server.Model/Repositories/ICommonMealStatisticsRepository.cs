using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealStatisticsRepository
    {
         Task<IImmutableList<CommonMealStatisticsOverview>> GetOverviewStatistics(DateTime fromDate, DateTime toDate);
    }
}