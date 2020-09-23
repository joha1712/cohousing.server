using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
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
        
        public async Task CreateDefaultMeals(DateTime date, int numDays, int numChefs, IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> defaultMealDates)
        {
            var lookup = defaultMealDates.ToImmutableDictionary(x => x.Key, y => y.Value);
            
            for (var dayIdx=0; dayIdx<numDays; dayIdx++)
            {
                var mealDate = date.Date.AddDays(dayIdx);

                if (!lookup.ContainsKey(mealDate.DayOfWeek))
                    continue;

                var timeOfDay = lookup[mealDate.DayOfWeek];
                mealDate = mealDate.Add(timeOfDay);

                var meal = await _commonMealRepository.GetByDate(mealDate);

                if (meal != null) 
                    continue;
                
                meal = await _commonMealFactory.Create(mealDate, numChefs);
                await _commonMealRepository.Add(meal);
            }
        }

        public async Task<IImmutableList<CommonMeal>> Load(DateTime mealDate, int numDays)
        {
            return await _commonMealRepository.GetByDateRange(mealDate, mealDate.AddDays(numDays));
        }
    }
}
