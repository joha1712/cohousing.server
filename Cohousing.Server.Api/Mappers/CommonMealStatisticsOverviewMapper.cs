using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealStatisticsOverviewMapper : ICommonMealStatisticsOverviewMapper
    {
        private readonly IPersonRepository _personRepository;

        public CommonMealStatisticsOverviewMapper(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public CommonMealStatisticsOverview Map(CommonMealStatisticsOverviewViewModel item)
        {
            return new CommonMealStatisticsOverview {
                PersonId = item.PersonId,
                ChefCount = item.ChefCount,
                MealCount = item.MealCount,
                AdultGuestsCount = item.AdultGuests,
                ChildGuestsCount = item.ChildGuests,
                Cost = new CommonMealStatisticsCostSumOverview {
                    ExpensesSum = item.ExpensesSum,
                    MealCostSum = item.MealCostSum
                }
            };
        }

        public async Task<CommonMealStatisticsOverviewViewModel> Map(CommonMealStatisticsOverview item)
        {           
            var person = await _personRepository.GetById(item.PersonId);

            return new CommonMealStatisticsOverviewViewModel {
                PersonId = item.PersonId,
                PersonName = person.CallName,
                ChefCount = item.ChefCount,
                MealCount =  item.MealCount,
                AdultGuests = item.AdultGuestsCount,
                ChildGuests = item.ChildGuestsCount,
                MealCostSum = item.Cost.MealCostSum,
                ExpensesSum = item.Cost.ExpensesSum
            };
        } 

        public async Task<IImmutableList<CommonMealStatisticsOverviewViewModel>> MapMany(IEnumerable<CommonMealStatisticsOverview> items) {
            var tasks = await Task.WhenAll(items.Select(Map));
            return tasks.ToImmutableList();
        }
        public IImmutableList<CommonMealStatisticsOverview> MapMany(IEnumerable<CommonMealStatisticsOverviewViewModel> items)
        {
             return items?.Select(Map).ToImmutableList();
        }
    }
}