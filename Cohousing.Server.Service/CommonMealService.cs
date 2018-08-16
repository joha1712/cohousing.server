using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        public async Task<IImmutableList<CommonMeal>> LoadOrCreate(DateTime date, int numDays, TimeSpan mealTime)
        {
            var result = new List<CommonMeal>();

            foreach (var dayIdx in Enumerable.Range(0, numDays))
            {
                var mealDate = date.AddDays(dayIdx).Add(mealTime);

                var meal = await _commonMealRepository.GetByDate(mealDate);


                if (meal == null)
                {
                    meal = await _commonMealFactory.Create(mealDate);
                    await _commonMealRepository.Add(meal);
                }

                result.Add(meal);
            }

            return result.ToImmutableList();
        }
    }
}
