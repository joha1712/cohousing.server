using System;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealShoppingInfoViewModel
    {
        public int MealId { get; set; }
        public DateTime Date { get; set; }

        public string DayName { get; set; }
        public string DateName { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Budget { get; set; }
        public IImmutableList<CommonMealExpenseViewModel> Expenses { get; set; }
    }
}