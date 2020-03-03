using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealStatisticOverviewMapper : ICommonMealStatisticOverviewMapper
    {
        private readonly IPersonRepository _personRepository;

        public CommonMealStatisticOverviewMapper(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }      

        public async Task<CommonMealStatisticOverviewViewModel> Map(CommonMealStatisticOverview item)
        {           
            var person = await _personRepository.GetById(item.PersonId);
            return Map(item, person);
        } 

        public async Task<IImmutableList<CommonMealStatisticOverviewViewModel>> MapMany(IEnumerable<CommonMealStatisticOverview> items) {
            var persons = await _personRepository.GetAll();
            var personLookup = persons.ToDictionary(x => x.Id, x => x);
            
            var result = items.Select(x => Map(x, personLookup[x.PersonId]));
            return result.ToImmutableList();
        }

        private CommonMealStatisticOverviewViewModel Map(CommonMealStatisticOverview item, Model.Models.Person person)
        {
            return new CommonMealStatisticOverviewViewModel {
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
    }
}