using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Common;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.Startup;
using Cohousing.Server.Api.ViewModels;
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
        private readonly ICommonMealMapper _commonMealMapper;
        private readonly ICommonMealSettings _commonMealSettings;

        public CommonMealController(ITimeProvider timeProvider, ICommonMealService commonMealService, ICommonMealMapper commonMealMapper, ICommonMealSettings commonMealSettings)
        {
            _timeProvider = timeProvider;
            _commonMealService = commonMealService;
            _commonMealMapper = commonMealMapper;
            _commonMealSettings = commonMealSettings;
        }

        // GET api/values
        [HttpGet("list")]
        public async Task<ActionResult<CommonMealViewModel[]>> List(DateTime? mealDate = null, int? numDays = null)
        {
            var dateOnly = (mealDate ?? _timeProvider.Now).Date;
            numDays = numDays ?? 5;

            var commonMeals = await _commonMealService.LoadOrCreate(dateOnly, numDays.Value, _commonMealSettings.MealTime);
            var result = _commonMealMapper.Map(commonMeals); 

            return result.ToArray();
        }
    }
}