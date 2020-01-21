using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealRepository : IRepository<CommonMeal>
    {
        Task<CommonMeal> GetByDate(DateTime date);
        Task<IImmutableList<CommonMeal>> GetByDateRange(DateTime dateFrom, DateTime dateTo);
        Task UpdateNote(int id, string note);
        Task UpdateStatus(int id, string status);
        Task<string> GetStatus(int mealId);
    }
}