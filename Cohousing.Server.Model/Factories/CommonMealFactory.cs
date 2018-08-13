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

        public CommonMealFactory(IPersonRepository personRepository, CommonMealRegistrationFactory commonMealRegistrationFactory)
        {
            _personRepository = personRepository;
            _commonMealRegistrationFactory = commonMealRegistrationFactory;
        }
        
        public async Task<CommonMeal> Create(DateTime date)
        {
            var persons = await _personRepository.GetAll();

            return new CommonMeal
            {
                Id = -1,
                Date = date,
                Registrations = _commonMealRegistrationFactory.Create(persons)
            };
        }
    }
}
