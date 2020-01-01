using System;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealsViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime WeekDate { get; set; }
        
        public IImmutableList<CommonMealViewModel> Meals { get; set; }
        public IImmutableList<PersonViewModel> Persons { get; set; }
    }
}
