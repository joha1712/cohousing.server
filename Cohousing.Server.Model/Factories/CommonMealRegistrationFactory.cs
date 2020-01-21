using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealRegistrationFactory
    {
        private readonly ITimeProvider _timeProvider;

        public CommonMealRegistrationFactory(ITimeProvider timeProvider) {
            _timeProvider = timeProvider;
        }

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
                Timestamp = _timeProvider.Now(),
                Guests = new CommonMealGuestRegistration {
                    RegistrationId = -1,
                    Adults = new PersonGroup(),
                    Children = new PersonGroup(),
                }
            };
        }
    }
}