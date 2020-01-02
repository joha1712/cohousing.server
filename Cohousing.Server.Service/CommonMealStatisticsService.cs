using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;

namespace Cohousing.Server.Service
{
    public class CommonMealStatisticsService : ICommonMealStatisticsService
    {
        private readonly ICommonMealStatisticsRepository _statisticsRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICommonMealPriceSettings _commonMealPriceSettings;

        public CommonMealStatisticsService(ICommonMealStatisticsRepository statisticsRepository, IPersonRepository personRepository, ICommonMealPriceSettings commonMealPriceSettings) {
            _statisticsRepository = statisticsRepository;
            _personRepository = personRepository;
            _commonMealPriceSettings = commonMealPriceSettings;
        }

        public async Task<IImmutableList<CommonMealStatisticOverview>> LoadOverview(DateTime fromDate, DateTime toDate) {
            fromDate = fromDate.StartOfDay();
            toDate = toDate.EndOfDay();
            
            var result = await _statisticsRepository.GetOverviewStatistics(fromDate, toDate);
            var prices = (await _personRepository.GetAll())
                .ToImmutableDictionary(x => x.Id, x => x.IsAdult() ? _commonMealPriceSettings.GetAdultPrice() : _commonMealPriceSettings.GetChildPrice());

            foreach (var r in result) {
                var mealCostPrice = prices[r.PersonId];
                r.Cost.MealCostSum = r.AdultGuestsCount * _commonMealPriceSettings.GetAdultPrice() 
                    + r.ChildGuestsCount * _commonMealPriceSettings.GetChildPrice() 
                    + r.MealCount * mealCostPrice;                
            }

            return result;
        }
    }
}