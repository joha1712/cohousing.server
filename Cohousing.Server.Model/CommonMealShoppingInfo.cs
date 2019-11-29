using System.Collections.Immutable;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model
{
    public class CommonMealShoppingInfo
    {
        public int MealId { get; set; }
        public int Budget { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public IImmutableList<CommonMealExpense> Expenses { get; set; }
    }
}