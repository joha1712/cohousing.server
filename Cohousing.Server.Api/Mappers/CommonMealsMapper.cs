using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealsMapper : ICommonMealsMapper
    {
        private readonly IPersonRepository _personRepository;
        public readonly ICommonMealMapper _commonMealMapper;

        public CommonMealsMapper(IPersonRepository personRepository, ICommonMealMapper commonMealMapper)
        {
            _personRepository = personRepository;
            _commonMealMapper = commonMealMapper;
        }

        public CommonMealsViewModel Map(IImmutableList<CommonMeal> meals)
        {
            var persons = _personRepository.GetAll().Result;

            return new CommonMealsViewModel
            {
                Meals = _commonMealMapper.MapMany(meals),
                Persons = persons.Select(x => new PersonViewModel
                {
                    Id = x.Id + "",
                    Name = $"{x.FirstName} {x.LastName}"
                }).ToImmutableList()
            };
        }
    }
}