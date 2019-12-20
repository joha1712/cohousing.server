using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Model;

namespace Cohousing.Server.Api.ViewModels
{
    public interface ICommonMealStatisticsOverviewMapper
    {   
        Task<CommonMealStatisticsOverviewViewModel> Map(CommonMealStatisticsOverview item);
        CommonMealStatisticsOverview Map(CommonMealStatisticsOverviewViewModel item);
        Task<IImmutableList<CommonMealStatisticsOverviewViewModel>> MapMany(IEnumerable<CommonMealStatisticsOverview> items);
        IImmutableList<CommonMealStatisticsOverview> MapMany(IEnumerable<CommonMealStatisticsOverviewViewModel> items);      
    }
}