using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Factories;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Service
{
    public class CommonMealService : ICommonMealService
    {
        private readonly ICommonMealRepository _commonMealRepository;
        private readonly CommonMealFactory _commonMealFactory;
        
        public CommonMealService(ICommonMealRepository commonMealRepository, CommonMealFactory commonMealFactory)
        {
            _commonMealRepository = commonMealRepository;
            _commonMealFactory = commonMealFactory;
        }

        public async Task<IImmutableList<CommonMeal>> LoadPrevious(DateTime date, int numPrevious = 1)
        {
            var meals = await _commonMealRepository.GetPreviousByDate(date, numPrevious);
            return meals.ToImmutableList();
        }

        public async Task<IImmutableList<CommonMeal>> LoadOrCreate(DateTime date, int numDays, int numChefs, IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> defaultMealDates)
        {
            var result = new List<CommonMeal>();
            var lookup = defaultMealDates.ToImmutableDictionary(x => x.Key, y => y.Value);
            
            for (var dayIdx=0; result.Count<numDays; dayIdx++)
            {
                var mealDate = date.Date.AddDays(dayIdx);

                if (!lookup.ContainsKey(mealDate.DayOfWeek))
                    continue;

                var timeOfDay = lookup[mealDate.DayOfWeek];
                mealDate = mealDate.Add(timeOfDay);

                var meal = await _commonMealRepository.GetByDate(mealDate);

                if (meal == null)
                {
                    meal = await _commonMealFactory.Create(mealDate, numChefs);
                    await _commonMealRepository.Add(meal);
                }

                result.Add(meal);
            }

            return result.ToImmutableList();
        }
    }
}
