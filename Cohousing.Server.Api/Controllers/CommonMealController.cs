using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.Startup;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Service;
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

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDays.Value, _commonMealSettings.DefaultNumberOfChefs, _commonMealSettings.DefaultMealTime);
            var result = _commonMealsMapper.Map(commonMeals);

            return result;
        }
    }
}