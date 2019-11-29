using System;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string DayName { get; set; }
        public string DateName { get; set; }
        public bool IsActiveMeal { get; set; }
        public string Note { get; set; }

        public IImmutableList<CommonMealRegistrationGroupViewModel> RegistrationGroups { get; set; }
        public IImmutableList<CommonMealChefViewModel> Chefs { get; set; }
    }
}