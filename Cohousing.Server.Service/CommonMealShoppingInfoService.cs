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

        public async Task<CommonMealShoppingInfo> Load(int mealId)
        {
            // Load the common meal
            var meal = await _commonMealRepository.GetById(mealId);
            var persons = (await _personRepository.GetAll()).ToDictionary(x => x.Id, x => x);

            // Load the existing expenses
            var expenses = (await _commonMealExpenseRepository
                    .GetByCommonMealId(mealId))
                .ToDictionary(x => x.PersonId, x => x);

            // Create missing expenses (each chef should have an expense record):
            var expectedExpenses = meal.Chefs
                .Where(x => x.PersonId != null)
                .Select(x => x.PersonId.Value)
                .ToImmutableList();

            foreach (var expectedExpense in expectedExpenses)
            {
                if (!expenses.ContainsKey(expectedExpense))
                {
                    // Create a new expense
                    expenses.Add(expectedExpense,
                        new CommonMealExpense
                        {
                            Amount = null,
                            MealId = mealId,
                            Timestamp = _timeProvider.Now(),
                            Id = -1,
                            PersonId = expectedExpense
                        });
                }
            }

            // Create the shopping info
            var adults = meal.Registrations.Count(x => x.Attending && persons[x.PersonId].IsAdult());
            var children = meal.Registrations.Count(x => x.Attending && persons[x.PersonId].IsChild());
            var vegetarians = meal.Registrations.Count(x => x.Attending && persons[x.PersonId].IsAdult() && persons[x.PersonId].IsVegetarian());
            var budget = (int)Math.Round((adults * _commonMealPriceSettings.GetAdultPrice()) + (children * _commonMealPriceSettings.GetChildPrice()), 0);

            return new CommonMealShoppingInfo
            {
                MealId = mealId,
                Adults = adults,
                Vegetarians = vegetarians,
                Children = children,
                Expenses = expenses.Values.ToImmutableList(),
                Budget = budget,
            };
        }
    }
}