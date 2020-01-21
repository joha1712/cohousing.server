using System;
using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealRegistrationMapper : MapperBase<CommonMealRegistration, CommonMealRegistrationViewModel>, ICommonMealRegistrationMapper
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICommonMealGuestRegistrationMapper _commonMealGuestRegistrationMapper;

        public CommonMealRegistrationMapper(IPersonRepository personRepository, ICommonMealGuestRegistrationMapper commonMealGuestRegistrationMapper)
        {
            _commonMealGuestRegistrationMapper = commonMealGuestRegistrationMapper;
            _personRepository = personRepository;
        }

        public override CommonMealRegistration Map(CommonMealRegistrationViewModel item)
        {
            return new CommonMealRegistration
            {
                Id = Convert.ToInt32(item.Id),
                CommonMealId = item.CommonMealId,
                Attending = item.Attending,
                PersonId = Convert.ToInt32(item.PersonId),
                Timestamp = DateTime.MinValue,
                Guests = _commonMealGuestRegistrationMapper.Map(item.Guests)
            };
        }

        public override CommonMealRegistrationViewModel Map(CommonMealRegistration item)
        {
            var person = _personRepository.GetById(item.PersonId).Result;

            return new CommonMealRegistrationViewModel
            {
                Id = item.Id,
                CommonMealId = item.CommonMealId,
                Attending = item.Attending,
                PersonId = item.PersonId,
                PersonName = person.CallName,
                Guests = _commonMealGuestRegistrationMapper.Map(item.Guests)
            };
        }
    }
}