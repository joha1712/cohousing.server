using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealExpenseRepository
    {
        Task<IImmutableList<CommonMealExpense>> GetByCommonMealId(int mealId);
        Task Update(CommonMealExpense expenseModel);
        Task<CommonMealExpense> Add(CommonMealExpense expenseModel);
    }
}