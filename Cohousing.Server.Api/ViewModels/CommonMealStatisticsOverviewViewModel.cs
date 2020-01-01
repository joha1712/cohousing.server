using System;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealStatisticsOverviewViewModel {
        public string Title {get; set;}
        public string Subtitle { get; set;}
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set;}
        public IImmutableList<CommonMealStatisticOverviewViewModel> Rows { get; set; }
    }
}