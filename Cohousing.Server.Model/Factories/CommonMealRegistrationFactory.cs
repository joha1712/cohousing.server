using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealRegistrationFactory
    {
        public IImmutableList<CommonMealRegistration> Create(IEnumerable<Person> persons)
        {
            var registrations = persons.Select(Create);
            return registrations.ToImmutableList();
        }

        public CommonMealRegistration Create(Person person)
        {
            return new CommonMealRegistration
            {
                Id = -1,
                Attending = false,
                PersonId = person.Id,
            };
        }
    }
}