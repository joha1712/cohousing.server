using System;
using System.Collections.Immutable;
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
        public async Task<ActionResult<CommonMealsViewModel>> List(DateTime? mealDate = null, int? numDays = null)
        {
            var dateOnly = (mealDate ?? _timeProvider.Now).Date;
            numDays = numDays ?? _commonMealSettings.DefaultDaysShown;

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDays.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultCommonMealDates);
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }

        [HttpGet("commonmeals/previous")]
        public async Task<ActionResult<CommonMealsViewModel>> Previous(DateTime? mealDate, int? numDays = null)
        {
            var dateOnly = (mealDate ?? _timeProvider.Now).Date;
            numDays = numDays ?? _commonMealSettings.DefaultDaysShown;

            var previousMeals = await _commonMealService.LoadPrevious(dateOnly, 1);
            var nextMeals = await _commonMealService.LoadOrCreate(dateOnly, numDays.Value - previousMeals.Count, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultCommonMealDates);
            
            var result = _commonMealsMapper.Map(previousMeals.Append(nextMeals));
            return result;
        }

        [HttpGet("commonmeals/next")]
        public async Task<ActionResult<CommonMealsViewModel>> Next(DateTime? mealDate, int? numDays = null)
        {
            if (mealDate == null)
                throw new Exception("Meal date missing");

            numDays = numDays ?? _commonMealSettings.DefaultDaysShown;
            var dateOnly = mealDate.Value.Date.AddDays(1);

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDays.Value, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultCommonMealDates);
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }
    }
}