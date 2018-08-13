using System;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.RestApi.Mappers.Common;
using Cohousing.Server.RestApi.ViewModels;

namespace Cohousing.Server.RestApi.Mappers
{
    public class CommonMealRegistrationMapper : MapperBase<CommonMealRegistration, CommonMealRegistrationViewModel>, ICommonMealRegistrationMapper
    {
        private readonly IPersonRepository _personRepository;

        public CommonMealRegistrationMapper(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public override CommonMealRegistration Map(CommonMealRegistrationViewModel item)
        {
            return new CommonMealRegistration
            {
                Id = Convert.ToInt32(item.Id),
                Attending = item.Attending,
                PersonId = Convert.ToInt32(item.PersonId)
            };
        }

        public override CommonMealRegistrationViewModel Map(CommonMealRegistration item)
        {
            var person = _personRepository.GetById(item.PersonId).Result;

            return new CommonMealRegistrationViewModel
            {
                Id = item.Id + "",
                Attending = item.Attending,
                PersonId = item.PersonId + "",
                PersonName = $"{person.FirstName} {person.LastName}"
            };
        }
    }
}