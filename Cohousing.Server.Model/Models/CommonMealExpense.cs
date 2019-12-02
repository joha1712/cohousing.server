using System;

namespace Cohousing.Server.Model.Models
{
    public class CommonMealExpense
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int MealId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? Amount { get; set; }
    }
}