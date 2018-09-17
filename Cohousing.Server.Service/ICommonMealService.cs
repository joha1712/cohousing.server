using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealService
    {
        Task<IImmutableList<CommonMeal>> LoadOrCreate(DateTime date, int numDays, int numChefs, IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> defaultMealDates);
        Task<IImmutableList<CommonMeal>> LoadPrevious(DateTime date, int numPrevious = 1);
    }
}