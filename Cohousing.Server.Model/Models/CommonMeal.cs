using System;
using System.Collections.Immutable;

namespace Cohousing.Server.Model.Models
{
    public class CommonMeal
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IImmutableList<CommonMealRegistration> Registrations { get; set; }
    }
}