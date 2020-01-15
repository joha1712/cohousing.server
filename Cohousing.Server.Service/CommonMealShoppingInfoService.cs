using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;


namespace Cohousing.Server.Service
{
    public class CommonMealShoppingInfoService : ICommonMealShoppingInfoService
    {
        private readonly ICommonMealRepository _commonMealRepository;
        private readonly ICommonMealExpenseRepository _commonMealExpenseRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly ICommonMealPriceSettings _commonMealPriceSettings;

        public CommonMealShoppingInfoService(ICommonMealRepository commonMealRepository, ICommonMealExpenseRepository commonMealExpenseRepository, ITimeProvider timeProvider, IPersonRepository personRepository, ICommonMealPriceSettings commonMealPriceSettings)
        {
            _commonMealPriceSettings = commonMealPriceSettings;
            _commonMealRepository = commonMealRepository;
            _commonMealExpenseRepository = commonMealExpenseRepository;
            _timeProvider = timeProvider;
            _personRepository = personRepository;
        }

        // TODO: Separate Load & Crate like in the CommonMealService
        public async Task<CommonMealShoppingInfo> Load(int mealId)
        {
            // Load the common meal, persons and guest regis
            var meal = await _commonMealRepository.GetById(mealId);
            
            // Load the existing expenses
            var allExpenses = (await _commonMealExpenseRepository
                    .GetByCommonMealId(mealId))
                .ToDictionary(x => x.PersonId, x => x);

            // Create missing expenses (each chef should have an expense record):
            var expenses = meal.Chefs
                .Where(x => x.PersonId != null)
                .Select(x => x.PersonId.Value)
                .Select(x => allExpenses.ContainsKey(x) ? allExpenses[x] : CreateDefaultExpense(x, mealId))
                .ToImmutableList();

            // Create the shopping info
            var (adults, children) = await CalcMealInfo(meal.Registrations);   
            var budget = (int)Math.Round((adults.Total * _commonMealPriceSettings.GetAdultPrice()) + (children.Total * _commonMealPriceSettings.GetChildPrice()), 0);

            return new CommonMealShoppingInfo
            {
                MealId = mealId,
                Adults = adults,
                Children = children,
                Expenses = expenses.ToImmutableList(),
                Budget = budget,
            };
        }

        private async Task<(PersonGroup adults, PersonGroup children)> CalcMealInfo(IEnumerable<CommonMealRegistration> registrations) {
            var adults = new PersonGroup { Conventional = 0, Vegetarians = 0};
            var chidren = new PersonGroup { Conventional = 0, Vegetarians = 0};

            var personCache = (await _personRepository.GetAll()).ToDictionary(x => x.Id, x => x);

            foreach (var reg in registrations) {
                adults.Vegetarians += reg.Guests?.Adults.Vegetarians ?? 0;
                adults.Conventional += reg.Guests?.Adults.Conventional ?? 0;
                chidren.Vegetarians += reg.Guests?.Children.Vegetarians ?? 0;
                chidren.Vegetarians += reg.Guests?.Children.Conventional ?? 0;

                if (reg.Attending) {
                    var person = personCache[reg.PersonId];

                    if (person.IsAdult()) {
                        if (person.IsVegetarian())
                            adults.Vegetarians += 1;
                        else
                            adults.Conventional += 1;
                    }
                    else if (person.IsChild()) {
                        if (person.IsVegetarian())
                            chidren.Vegetarians += 1;
                        else
                            chidren.Conventional += 1;
                    }
                }
            }

            return (adults, chidren);
        }

        private CommonMealExpense CreateDefaultExpense(int personId, int mealId) {
            return new CommonMealExpense
                {
                    Amount = null,
                    MealId = mealId,
                    Timestamp = _timeProvider.Now(),
                    Id = -1,
                    PersonId = personId
                };
        }
    }
}