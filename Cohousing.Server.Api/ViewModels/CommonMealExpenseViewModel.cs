using System;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealExpenseViewModel
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int? Amount { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime Date { get; set; }
    }
}