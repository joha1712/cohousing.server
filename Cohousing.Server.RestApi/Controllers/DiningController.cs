using System;
using System.Collections.Generic;
using System.Linq;
using Cohousing.Common;
using Cohousing.WebSite.RestApi.Common;
using Cohousing.WebSite.RestApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.WebSite.RestApi.Controllers
{
    [Route("api/dining")]
    [ApiController]
    public class DiningController : ControllerBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ITimeFormatter _timeFormatter;

        public DiningController(ITimeProvider timeProvider, ITimeFormatter timeFormatter)
        {
            _timeProvider = timeProvider;
            _timeFormatter = timeFormatter;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<DiningViewModel> Get(DateTime? dateUtc)
        {
            dateUtc = dateUtc ?? _timeProvider.Now;

            var dates = Enumerable
                .Range(0, 6)
                .Select(x => dateUtc.Value.AddDays(x))
                .ToList();

            var result = new DiningViewModel
            {
                DinningDays = dates.Select(d => CreateDiningDay(d, _timeFormatter)).ToList()
            };

            return result;
        }

        private static DiningDayViewModel CreateDiningDay(DateTime date, ITimeFormatter timeFormatter)
        {
            return new DiningDayViewModel
            {
                Id = "1",
                Date = date,
                DateName = timeFormatter.GetDateName(date),
                DayName = timeFormatter.GetDayName(date).ToUpperFirstLetter(),
                Persons = new List<DiningPersonViewModel>
                {
                    new DiningPersonViewModel
                    {
                        Id = "1",
                        Attending = true,
                        Name = "Jonas"
                    },
                    new DiningPersonViewModel
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
