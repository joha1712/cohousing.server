using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.Startup;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Service;
using Cohousing.Server.Util;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeal")]
    [ApiController]
    public class CommonMealController : ControllerBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ICommonMealService _commonMealService;
        private readonly ICommonMealsMapper _commonMealsMapper;
        private readonly ICommonMealSettings _commonMealSettings;
        
        public CommonMealController(ITimeProvider timeProvider, ICommonMealService commonMealService, ICommonMealsMapper commonMealsMapper, ICommonMealSettings commonMealSettings)
        {
            _timeProvider = timeProvider;
            _commonMealService = commonMealService;
            _commonMealsMapper = commonMealsMapper;
            _commonMealSettings = commonMealSettings;
        }
        
        // GET api/values
        [HttpGet("commonmeals/week")]
        public async Task<ActionResult<CommonMealsViewModel>> List(DateTime weekDate)
        {
            var numDaysToLoad = _commonMealSettings.DefaultDaysToLoad;
            var startOfWeekDate = weekDate.StartOfWeekDate().Date;

            await _commonMealService.CreateDefaultMeals(startOfWeekDate, numDaysToLoad, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(startOfWeekDate, numDaysToLoad);
            
            var result = _commonMealsMapper.Map(commonMeals, startOfWeekDate);
            return result;
        }

        // GET api/values
        [HttpGet("commonmeals/activeweek")]
        public async Task<ActionResult<CommonMealsViewModel>> List()
        {
            var startOfWeekDate = _timeProvider.Now.StartOfWeekDate().Date;

            var result = await List(startOfWeekDate);
           
            // Did we get any active meals - if not fetch next week
            if (result.Value.Meals.Last().Date <= _timeProvider.Now)
            {
                startOfWeekDate = startOfWeekDate.AddDays(7);
                result = await List(startOfWeekDate);
            }
            
            return result;
        }

        [HttpGet("commonmeals/previousweek")]
        public async Task<ActionResult<CommonMealsViewModel>> Previous(DateTime weekDate)
        {
            if (weekDate == null) throw new Exception("Week date missing");
            
            var numDaysBack = _commonMealSettings.DefaultDaysToLoad;
            var numDaysToLoad = _commonMealSettings.DefaultDaysToLoad;
            var previousWeekDate = weekDate.Date.AddDays(-numDaysBack);            

            await _commonMealService.CreateDefaultMeals(previousWeekDate, numDaysToLoad, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(previousWeekDate, numDaysToLoad);
            
            var result = _commonMealsMapper.Map(commonMeals, previousWeekDate);
            return result;
        }

        [HttpGet("commonmeals/nextweek")]
        public async Task<ActionResult<CommonMealsViewModel>> Next(DateTime weekDate)
        {
            if (weekDate == null) throw new Exception("Week date missing");

            var numDaysAhead = _commonMealSettings.DefaultDaysToLoad;
            var numDaysToLoad = _commonMealSettings.DefaultDaysToLoad;
            var nextWeekDate = weekDate.Date.AddDays(numDaysAhead);

            await _commonMealService.CreateDefaultMeals(nextWeekDate, numDaysToLoad, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(nextWeekDate, numDaysToLoad);
            
            var result = _commonMealsMapper.Map(commonMeals, nextWeekDate);
            return result;
        }
    }
}