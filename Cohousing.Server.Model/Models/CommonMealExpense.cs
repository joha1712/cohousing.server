using System;

namespace Cohousing.Server.Model.Models
{
    public class CommonMealExpense
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int CommonMealId { get; set; }
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }
    }
}