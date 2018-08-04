using System;
using System.Collections.Generic;

namespace Cohousing.WebSite.RestApi.ViewModels
{
    public class CommonMealDayViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public string DayName { get; set; }
        public string DateName { get; set; }

        public IList<CommonMealPersonViewModel> Persons { get; set; }
    }
}