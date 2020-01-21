using System;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealFactory
    {
        private readonly IPersonRepository _personRepository;
        private readonly CommonMealRegistrationFactory _commonMealRegistrationFactory;
        private readonly CommonMealChefFactory _commonMealChefFactory;

        public CommonMealFactory(IPersonRepository personRepository, CommonMealRegistrationFactory commonMealRegistrationFactory, CommonMealChefFactory commonMealChefFactory)
        {
            _personRepository = personRepository;
            _commonMealRegistrationFactory = commonMealRegistrationFactory;
            _commonMealChefFactory = commonMealChefFactory;
        }
        
        public async Task<CommonMeal> Create(DateTime date, int numChefs)
        {
            var persons = await _personRepository.GetAll();

            return new CommonMeal
            {
                Id = -1,
                Date = date,
                Registrations = _commonMealRegistrationFactory.CreateMany(persons),
                Chefs = _commonMealChefFactory.CreateMany(numChefs),
                Note = null,
                Status = "OPEN"
            };
        }
    }
}
