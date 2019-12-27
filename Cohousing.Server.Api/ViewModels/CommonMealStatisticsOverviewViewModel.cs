namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealStatisticsOverviewViewModel
    {
        public int PersonId { get; set;}
        
        public string PersonName { get; set; }
        public int ChefCount { get; set; }
        public int MealCount { get; set; }
        public int AdultGuests { get; set; }
        public int ChildGuests { get; set; }
        public decimal ExpensesSum { get; set; }
        public decimal MealCostSum { get; set; }
        public decimal NettoSum => MealCostSum - ExpensesSum;
    }
}