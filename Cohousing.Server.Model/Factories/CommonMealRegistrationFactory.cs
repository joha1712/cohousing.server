using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealRegistrationFactory
    {
        public IImmutableList<CommonMealRegistration> CreateMany(IEnumerable<Person> persons)
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
                Guests = new CommonMealGuestRegistration {
                    RegistrationId = -1,
                    Adults = new PersonGroup(),
                    Children = new PersonGroup(),
                }
            };
        }
    }
}