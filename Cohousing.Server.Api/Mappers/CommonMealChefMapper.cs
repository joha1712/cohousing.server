using System;
using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealChefMapper : MapperBase<CommonMealChef, CommonMealChefViewModel>, ICommonMealChefMapper
    {
        private readonly IPersonRepository _personRepository;

        public CommonMealChefMapper(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public override CommonMealChef Map(CommonMealChefViewModel item)
        {
            return new CommonMealChef
            {
                Id = item.Id,                
                CommonMealId = item.CommonMealId,
                PersonId = item.PersonId,
                Timestamp = DateTime.MinValue,
            };
        }

        public override CommonMealChefViewModel Map(CommonMealChef item)
        {
            var person = item.PersonId != null
                ? _personRepository.GetById(item.PersonId.Value).Result
                : null;

            return new CommonMealChefViewModel
            {
                Id = item.Id,
                CommonMealId = item.CommonMealId,
                PersonId = person?.Id,
                PersonName = person?.FirstName
            };
        }
    }
}