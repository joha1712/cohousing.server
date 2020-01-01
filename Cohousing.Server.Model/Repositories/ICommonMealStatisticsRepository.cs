using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealStatisticsRepository
    {
         Task<IImmutableList<CommonMealStatisticOverview>> GetOverviewStatistics(DateTime fromDate, DateTime toDate);
    }
}