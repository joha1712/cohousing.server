using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealStatisticsOverviewMapper : ICommonMealStatisticsOverviewMapper
    {
        private readonly ICommonMealStatisticOverviewMapper _overviewMapper;

        public CommonMealStatisticsOverviewMapper(ICommonMealStatisticOverviewMapper overviewMapper) {
            _overviewMapper = overviewMapper;
        }

        public async Task<CommonMealStatisticsOverviewViewModel> Map(IEnumerable<CommonMealStatisticOverview> items, DateTime fromDate, DateTime toDate) {
            return new CommonMealStatisticsOverviewViewModel {
                Title = "Madplan opgørelse",
                Subtitle = "Buske bofællesskab",
                FromDate = fromDate,
                ToDate = toDate,
                Rows = await _overviewMapper.MapMany(items)
            };
        }    
    }
}