using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealService
    {
        Task<IImmutableList<CommonMeal>> LoadOrCreate(DateTime date, int numDays, TimeSpan mealTime);
    }
}