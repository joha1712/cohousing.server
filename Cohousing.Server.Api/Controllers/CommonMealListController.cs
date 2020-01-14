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
    [Route("api/commonmeals")]
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
        [HttpGet("list/week")]
        public async Task<ActionResult<CommonMealsViewModel>> List(DateTime weekDate, string sortExpr = null)
        {
            const int numDaysToLoad = 7;
            var startOfWeekDate = weekDate.StartOfWeekDate().Date;

            await _commonMealService.CreateDefaultMeals(startOfWeekDate, numDaysToLoad, _commonMealSettings.NumberOfChefs, _commonMealSettings.DefaultDinnerDates);
            var commonMeals = await _commonMealService.Load(startOfWeekDate, numDaysToLoad);
            
            var result = await _commonMealsMapper.Map(commonMeals, startOfWeekDate);
            SortRegistrationsGroups(result, sortExpr);
            SortPeople(result, sortExpr);
            
            return result;
        }

        // GET api/values
        [HttpGet("list/activeweek")]
        public async Task<ActionResult<CommonMealsViewModel>> List(string sortExpr = "")
        {
            var startOfWeekDate = _timeProvider.Now().StartOfWeekDate().Date;
            var result = await List(startOfWeekDate, sortExpr);
           
            // Did we get any active meals - if so we are done
            if (result.Value.Meals.Last().Date > _timeProvider.Now()) 
                return result;
            
            // Else fetch next week
            startOfWeekDate = startOfWeekDate.AddDays(7);
            result = await List(startOfWeekDate, sortExpr);

            return result;
        }

        [HttpGet("list/previousweek")]
        public async Task<ActionResult<CommonMealsViewModel>> Previous(DateTime weekDate, string sortExpr = null)
        {
            if (weekDate == null) throw new Exception("Week date missing");
            
            var previousWeekDate = weekDate.Date.AddDays(-7);
            return await List(previousWeekDate, sortExpr);
        }

        [HttpGet("list/nextweek")]
        public async Task<ActionResult<CommonMealsViewModel>> Next(DateTime weekDate, string sortExpr = null)
        {
            if (weekDate == null) throw new Exception("Week date missing");

            var nextWeekDate = weekDate.Date.AddDays(7);
            return await List(nextWeekDate, sortExpr);
        }
        
        private static void SortRegistrationsGroups(CommonMealsViewModel meals, string sortExpr)
        {
            if (string.IsNullOrWhiteSpace(sortExpr) || meals?.Meals == null)
                return;
            
            foreach (var meal in meals.Meals)
            {
                meal.RegistrationGroups = meal.RegistrationGroups
                    .OrderBy(rg => rg.Registrations
                        .Any(x => x.PersonName.ContainsOrdinalIgnoreCase(sortExpr)) ? "_" + rg.Name : rg.Name)
                    .ToImmutableList();
            }
        }         

        private static void SortPeople(CommonMealsViewModel meals, string sortExpr)
        {
            if (string.IsNullOrWhiteSpace(sortExpr) || meals?.People == null)
                return;            
           
            meals.People = meals.People
                .Select((p,idx) => new { Value = p, Idx = idx })
                .OrderBy(x => x.Value.Name.ContainsOrdinalIgnoreCase(sortExpr) ? -1 : x.Idx)
                .Select(x => x.Value)
                .ToImmutableList();
            
        }               
    }
}