using System;
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
        [HttpGet("commonmeals")]
        public async Task<ActionResult<CommonMealsViewModel>> List(DateTime? mealDate = null, int? numDaysToLoad = null)
        {
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var dateOnly = (mealDate ?? _timeProvider.Now.StartOfWeekDate()).Date;

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }

        [HttpGet("commonmeals/previous")]
        public async Task<ActionResult<CommonMealsViewModel>> Previous(DateTime mealDate, int? numDaysBack = null, int? numDaysToLoad = null)
        {
            if (mealDate == null) throw new Exception("Meal date missing");
            
            numDaysBack = numDaysBack ?? _commonMealSettings.DefaultDaysToLoad;
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var dateOnly = mealDate.Date.AddDays(-numDaysBack.Value);            

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            
            var result = _commonMealsMapper.Map(commonMeals);
            return result;
        }

        [HttpGet("commonmeals/next")]
        public async Task<ActionResult<CommonMealsViewModel>> Next(DateTime mealDate, int? numDaysAhead = null, int? numDaysToLoad = null)
        {
            if (mealDate == null) throw new Exception("Meal date missing");

            numDaysAhead = numDaysAhead ?? _commonMealSettings.DefaultDaysToLoad;
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var dateOnly = mealDate.Date.AddDays(numDaysAhead.Value);

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }
    }
}