using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cohousing.Server.RestApi.ViewModels
{
    public class CommonMealViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public string DayName { get; set; }
        public string DateName { get; set; }

        public IImmutableList<CommonMealRegistrationViewModel> Registrations { get; set; }
    }
}