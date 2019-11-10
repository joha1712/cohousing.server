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
        [HttpGet("commonmeals")]
        public async Task<ActionResult<CommonMealsViewModel>> List(DateTime? mealDate = null, int? numDaysToLoad = null)
        {
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var mealDateOnly = (mealDate ?? _timeProvider.Now.StartOfWeekDate()).Date;

            await _commonMealService.CreateDefaultMeals(mealDateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(mealDateOnly, numDaysToLoad.Value);

            if (mealDate == null && commonMeals.Last().Date <= _timeProvider.Now)
            {
                mealDateOnly = mealDateOnly.AddDays(7);
                await _commonMealService.CreateDefaultMeals(mealDateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
                commonMeals = await _commonMealService.Load(mealDateOnly, numDaysToLoad.Value);
            }
            
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }

        [HttpGet("commonmeals/previous")]
        public async Task<ActionResult<CommonMealsViewModel>> Previous(DateTime mealDate, int? numDaysBack = null, int? numDaysToLoad = null)
        {
            if (mealDate == null) throw new Exception("Meal date missing");
            
            numDaysBack = numDaysBack ?? _commonMealSettings.DefaultDaysToLoad;
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var mealDateOnly = mealDate.Date.AddDays(-numDaysBack.Value);            

            await _commonMealService.CreateDefaultMeals(mealDateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(mealDateOnly, numDaysToLoad.Value);
            
            var result = _commonMealsMapper.Map(commonMeals);
            return result;
        }

        [HttpGet("commonmeals/next")]
        public async Task<ActionResult<CommonMealsViewModel>> Next(DateTime mealDate, int? numDaysAhead = null, int? numDaysToLoad = null)
        {
            if (mealDate == null) throw new Exception("Meal date missing");

            numDaysAhead = numDaysAhead ?? _commonMealSettings.DefaultDaysToLoad;
            numDaysToLoad = numDaysToLoad ?? _commonMealSettings.DefaultDaysToLoad;
            var mealDateOnly = mealDate.Date.AddDays(numDaysAhead.Value);

            await _commonMealService.CreateDefaultMeals(mealDateOnly, numDaysToLoad.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(mealDateOnly, numDaysToLoad.Value);
            
            var result = _commonMealsMapper.Map(commonMeals);
            return result;
        }
    }
}