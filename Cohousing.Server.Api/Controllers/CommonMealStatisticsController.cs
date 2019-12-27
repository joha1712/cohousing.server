using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals/statistics")]
    [ApiController]
    public class CommonMealStatisticsController : ControllerBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ICommonMealStatisticsService _statisticsService;
        private readonly ICommonMealStatisticsOverviewMapper _statisticsOverviewMapper;

        public CommonMealStatisticsController(ICommonMealStatisticsService statisticsService, ICommonMealStatisticsOverviewMapper statisticsOverviewMapper, ITimeProvider timeProvider)
        {
            _statisticsService = statisticsService;
            _statisticsOverviewMapper = statisticsOverviewMapper;
            _timeProvider = timeProvider;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<IEnumerable<CommonMealStatisticsOverviewViewModel>>> Overview(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var now = _timeProvider.Now();
            var from = fromDate ?? now.AddDays(-now.Day).AddMonths(-1);
            var to = toDate ?? now;            

            var statistics = await _statisticsService.LoadOverview(from, to);
            var result =  await _statisticsOverviewMapper.MapMany(statistics);
            return result.ToImmutableList();
        }
    }    
}