using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Common;
using Cohousing.Server.Api.Mappers;
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

        public CommonMealController(ITimeProvider timeProvider, ICommonMealService commonMealService, ICommonMealMapper commonMealMapper)
        {
            _timeProvider = timeProvider;
            _commonMealService = commonMealService;
            _commonMealMapper = commonMealMapper;
        }

        // GET api/values
        [HttpGet("list")]
        public async Task<ActionResult<CommonMealViewModel[]>> List(DateTime? dateUtc = null, int? numDays = null)
        {
            dateUtc = dateUtc ?? _timeProvider.Now;
            numDays = numDays ?? 5;

            var commonMeals = await _commonMealService.LoadOrCreate(dateUtc.Value, numDays.Value);
            var result = _commonMealMapper.Map(commonMeals); 

            return result.ToArray();
        }
    }
}