using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Service;
using Cohousing.Server.Util;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealShoppingInfoController : ControllerBase
    {
        private readonly ICommonMealRepository _commonMealRepository;
        private readonly ITimeFormatter _timeFormatter;
        private readonly ICommonMealShoppingInfoService _commonMealShoppingInfoService;

        public CommonMealShoppingInfoController(ICommonMealRepository commonMealRepository, ITimeFormatter timeFormatter, ICommonMealShoppingInfoService commonMealShoppingInfoService)
        {
            _commonMealRepository = commonMealRepository;
            _timeFormatter = timeFormatter;
            _commonMealShoppingInfoService = commonMealShoppingInfoService;
        }

        // GET api/values
        [HttpGet("shoppinginfo")]
        public async Task<ActionResult<CommonMealShoppingInfoViewModel>> Get(int mealId)
        {
            var f = _commonMealShoppingInfoService.Load(mealId);
            
            var meal = await _commonMealRepository.GetById(mealId);

            var adults = meal.Registrations.Count(x => x.Attending);
            var children = meal.Registrations.Count(x => x.Attending);
            var budget = adults * 25;
            
            return new CommonMealShoppingInfoViewModel
            {
                MealId = meal.Id,
                Date = meal.Date,
                DateName = _timeFormatter.GetDateName(meal.Date),
                DayName = _timeFormatter.GetDayName(meal.Date).ToUpperFirstLetter(),
                Adults = adults,
                Children = children,
                Budget = budget,
                Amount = null
            };
        }
    }
}