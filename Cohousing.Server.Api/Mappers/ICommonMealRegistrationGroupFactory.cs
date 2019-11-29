using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Api.Mappers
{
    public interface ICommonMealRegistrationGroupFactory
    {
        IImmutableList<CommonMealRegistrationGroupViewModel> CreateGroups(IEnumerable<CommonMealRegistrationViewModel> registrations);
    }

    public class CommonMealRegistrationGroupFactory : ICommonMealRegistrationGroupFactory
    {
        private readonly Lazy<ImmutableDictionary<int, int>> _cache;

        public CommonMealRegistrationGroupFactory(IPersonRepository personRepository)
        {
            _cache = new Lazy<ImmutableDictionary<int, int>>(() =>
                {
                    return personRepository
                        .GetAll()
                        .Result
                        .ToImmutableDictionary(x => x.Id, x => x.AddressId);
                });
        }

        public IImmutableList<CommonMealRegistrationGroupViewModel> CreateGroups(IEnumerable<CommonMealRegistrationViewModel> registrations)
        {
            return registrations
                .GroupBy(reg => _cache.Value[reg.PersonId])
                .OrderBy(grp => grp.Key)
                .Select((grp,idx) => 
                    new CommonMealRegistrationGroupViewModel
                    {
                        GroupNo = idx,
                        Name = grp.Key + "",
                        Registrations = grp.ToImmutableList()
                    }
                ).ToImmutableList();
        }
    }
}