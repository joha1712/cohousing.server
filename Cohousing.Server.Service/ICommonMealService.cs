using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealService
    {
        Task<IImmutableList<CommonMeal>> Load(DateTime date, int numDays);
        Task CreateDefaultMeals(DateTime startDate, int numDays, int numChefs, IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> defaultMealDates);
    }

    public interface ICommonMealPriceSettings {
        decimal GetAdultPrice();
        decimal GetChildPrice();
    }
}