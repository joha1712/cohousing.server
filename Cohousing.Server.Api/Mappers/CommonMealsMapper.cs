using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealsMapper : ICommonMealsMapper
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICommonMealMapper _commonMealMapper;
        private readonly ITimeProvider _timeProvider;

        public CommonMealsMapper(IPersonRepository personRepository, ICommonMealMapper commonMealMapper, ITimeProvider timeProvider)
        {
            _personRepository = personRepository;
            _commonMealMapper = commonMealMapper;
            _timeProvider = timeProvider;
        }

        public CommonMealsViewModel Map(IImmutableList<CommonMeal> meals)
        {
            var persons = _personRepository.GetAll().Result;
            var weekNo = meals.FirstOrDefault()?.Date.GetIso8601WeekNo().ToString() ?? "{ukendt}";

            return new CommonMealsViewModel
            {
                Title = $"Fællesspisning - uge {weekNo}",
                Meals = _commonMealMapper.MapMany(meals),
                Persons = persons.Select(x => new PersonViewModel
                {
                    Id = x.Id + "",
                    Name = x.CallName
                }).ToImmutableList()
            };
        }
    }
}