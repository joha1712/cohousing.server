using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Api.ViewModels
{
    public interface ICommonMealStatisticsOverviewMapper
    {   
        Task<CommonMealStatisticsOverviewViewModel> Map(IEnumerable<CommonMealStatisticOverview> items, DateTime fromDate, DateTime toDate);    
    }
}