using System;
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
            var persons = (await _personRepository.GetAll()).ToDictionary(x => x.Id, x => x);

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
            var adults = meal.Registrations.Where(x => x.Attending && persons[x.PersonId].IsAdult()).ToList();
            var children = meal.Registrations.Where(x => x.Attending && persons[x.PersonId].IsChild()).ToList();
            var budget = (int)Math.Round((adults.Count * _commonMealPriceSettings.GetAdultPrice()) + (children.Count * _commonMealPriceSettings.GetChildPrice()), 0);

            return new CommonMealShoppingInfo
            {
                MealId = mealId,
                Adults = new PersonGroup { 
                    Conventional = adults.Count(x => !persons[x.PersonId].IsVegetarian()),
                    Vegetarians = adults.Count(x => persons[x.PersonId].IsVegetarian())},                
                Children = new PersonGroup { 
                    Conventional = children.Count(x => !persons[x.PersonId].IsVegetarian()), 
                    Vegetarians = children.Count(x => persons[x.PersonId].IsVegetarian())},
                Expenses = expenses.ToImmutableList(),
                Budget = budget,
            };
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