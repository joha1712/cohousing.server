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
        private readonly ITimeProvider _timeProvider;

        public CommonMealShoppingInfoService(ICommonMealRepository commonMealRepository, ICommonMealExpenseRepository commonMealExpenseRepository, ITimeProvider timeProvider)
        {
            _commonMealRepository = commonMealRepository;
            _commonMealExpenseRepository = commonMealExpenseRepository;
            _timeProvider = timeProvider;
        }
        
        public async Task<object> Load(int mealId)
        {
            // Load the common meal
            var meal = await _commonMealRepository.GetById(mealId);
            
            // Load the existing expenses
            var expenses = Enumerable.ToDictionary<CommonMealExpense, int, CommonMealExpense>((await _commonMealExpenseRepository
                    .GetByMealId(mealId)), x => x.PersonId, x => x);
            
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
                            CommonMealId = mealId,
                            Date = _timeProvider.Now(),
                            Id = -1,
                            PersonId = expectedExpense
                        });
                }
            }

            // Create the shopping info
            var adults = meal.Registrations.Count(x => x.Attending);
            var children = meal.Registrations.Count(x => x.Attending);
            var budget = adults * 25;

            return new CommonMealShoppingInfo
            {
                Expenses = expenses.Values.ToImmutableList(),
                Budget = budget,
                Adults = adults,
                Children = children
            };
        }
    }
}