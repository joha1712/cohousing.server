using System;

namespace Cohousing.Server.Model.Models
{
    public class CommonMealChef
    {
        public int Id { get; set; }
        public int CommonMealId { get; set; }
        public DateTime Timestamp { get; set; }
        public int? PersonId { get; set; }
    }
}