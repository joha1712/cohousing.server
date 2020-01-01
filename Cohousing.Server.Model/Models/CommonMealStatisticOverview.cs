namespace Cohousing.Server.Model
{
    public class CommonMealStatisticOverview
    {
        public int PersonId { get; set;}
        public int ChefCount { get; set; }
        public int MealCount { get; set; }
        public int AdultGuestsCount { get; set; }
        public int ChildGuestsCount { get; set; }
        public CommonMealStatisticCostSumOverview Cost { get; set; } 
    }

    public class CommonMealStatisticCostSumOverview {
        public decimal MealCostSum { get; set; }
        public decimal ExpensesSum { get; set; }
    }
}