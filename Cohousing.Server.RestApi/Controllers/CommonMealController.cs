using System;
using System.Collections.Generic;
using System.Linq;
using Cohousing.Common;
using Cohousing.WebSite.RestApi.Common;
using Cohousing.WebSite.RestApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.WebSite.RestApi.Controllers
{
    [Route("api/commonmeal")]
    [ApiController]
    public class CommonMealController : ControllerBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ITimeFormatter _timeFormatter;

        public CommonMealController(ITimeProvider timeProvider, ITimeFormatter timeFormatter)
        {
            _timeProvider = timeProvider;
            _timeFormatter = timeFormatter;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<CommonMealViewModel> Get(DateTime? dateUtc)
        {
            dateUtc = dateUtc ?? _timeProvider.Now;

            var dates = Enumerable
                .Range(0, 6)
                .Select(x => dateUtc.Value.AddDays(x))
                .ToList();

            var result = new CommonMealViewModel
            {
                DinningDays = dates.Select(d => CreateMealDay(d, _timeFormatter)).ToList()
            };

            return result;
        }

        private static CommonMealDayViewModel CreateMealDay(DateTime date, ITimeFormatter timeFormatter)
        {
            return new CommonMealDayViewModel
            {
                Id = "1",
                Date = date,
                DateName = timeFormatter.GetDateName(date),
                DayName = timeFormatter.GetDayName(date).ToUpperFirstLetter(),
                Persons = new List<CommonMealPersonViewModel>
                {
                    new CommonMealPersonViewModel
                    {
                        Id = "1",
                        Attending = true,
                        Name = "Jonas"
                    },
                    new CommonMealPersonViewModel
                    {
                        Id = "2",
                        Attending = false,
                        Name = "Eva"
                    }
                }
            };
        }
    }
}
