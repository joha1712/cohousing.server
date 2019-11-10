using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Factories;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;

namespace Cohousing.Server.Service
{
    public class CommonMealService : ICommonMealService
    {
        private readonly ICommonMealRepository _commonMealRepository;
        private readonly CommonMealFactory _commonMealFactory;
        private readonly ITimeProvider _timeProvider;

        public CommonMealService(ICommonMealRepository commonMealRepository, CommonMealFactory commonMealFactory, ITimeProvider timeProvider)
        {
            _commonMealRepository = commonMealRepository;
            _commonMealFactory = commonMealFactory;
            _timeProvider = timeProvider;
        }
        
        public async Task CreateDefaultMeals(DateTime date, int numDays, int numChefs, IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> defaultMealDates)
        {
            var result = new List<CommonMeal>();
            var lookup = defaultMealDates.ToImmutableDictionary(x => x.Key, y => y.Value);
            
            for (var dayIdx=0; dayIdx<numDays; dayIdx++)
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
        }

        public async Task<IImmutableList<CommonMeal>> Load(DateTime mealDate, int numDays)
        {
            return await _commonMealRepository.GetByDateRange(mealDate, mealDate.AddDays(numDays));
        }
    }
}
