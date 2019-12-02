using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealExpenseMapper : ICommonMealExpenseMapper
    {
        private readonly IPersonRepository _personRepository;

        public CommonMealExpenseMapper(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public CommonMealExpense Map(CommonMealExpenseViewModel item)
        {
            return new CommonMealExpense
            {
                Id = item.Id,
                PersonId = item.PersonId,
                Amount = item.Amount ?? 0,
                MealId = item.MealId,
                Timestamp = item.Date
            };
        }

        public IImmutableList<CommonMealExpense> MapMany(IImmutableList<CommonMealExpenseViewModel> items)
        {
            return items?.Select(Map).ToImmutableList();
        }

        public async Task<CommonMealExpenseViewModel> Map(CommonMealExpense item)
        {
            var person = await _personRepository.GetById(item.PersonId);
            
            return new CommonMealExpenseViewModel
            {
                Id = item.Id,
                Amount = item.Amount != null ? (int?) Math.Round(item.Amount.Value, 0) : null, 
                Date = item.Timestamp,
                MealId = item.MealId,
                PersonId = item.PersonId,
                PersonName = person.CallName
            };
        }

        public async Task<IImmutableList<CommonMealExpenseViewModel>> MapMany(IImmutableList<CommonMealExpense> items)
        {
            var tasks = items?.Select(Map);
            var result = await Task.WhenAll(tasks);
            return result.ToImmutableList();
        }
    }
}