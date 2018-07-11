using System;
using System.Collections.Generic;

namespace Cohousing.WebSite.RestApi.ViewModels
{
    public class DiningViewModel
    {
        public IList<DiningDayViewModel> DinningDays { get; set; }
    }

    public class DiningDayViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public string DayName { get; set; }
        public string DateName { get; set; }

        public IList<DiningPersonViewModel> Persons { get; set; }
    }

    public class DiningPersonViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Attending { get; set; }
    }
}
