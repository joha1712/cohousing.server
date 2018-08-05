using System.Collections.Generic;

namespace Cohousing.Server.RestApi.ViewModels
{
    public class CommonMealViewModel
    {
        public IList<CommonMealDayViewModel> MealDays { get; set; }
    }
}
