using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Service
{
    public interface ICommonMealStatisticsService
    {
         Task<IImmutableList<CommonMealStatisticOverview>> LoadOverview(DateTime fromDate, DateTime toDate);
    }
}