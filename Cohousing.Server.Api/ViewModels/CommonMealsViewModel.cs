using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealsViewModel
    {
        public IImmutableList<CommonMealViewModel> Meals { get; set; }
        public IImmutableList<PersonViewModel> Persons { get; set; }
    }
}
