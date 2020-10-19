using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.Startup;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Service
{
    public class CommonMealRegistrationService : ICommonMealRegistrationService
    {
        private readonly ICommonMealRegistrationRepository _registrationRepository;
        private readonly ICommonMealRepository _mealRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly ICommonMealSettings _commonMealSettings;
        private readonly IPersonRepository _personRepository;

        public CommonMealRegistrationService(ICommonMealRegistrationRepository registrationRepository, ICommonMealRepository mealRepository, ITimeProvider timeProvider, ICommonMealSettings commonMealSettings, IPersonRepository personRepository)
        {
            _registrationRepository = registrationRepository;
            _mealRepository = mealRepository;
            _timeProvider = timeProvider;
            _commonMealSettings = commonMealSettings;
            _personRepository = personRepository;
        }

        public async Task<CommonMealRegistration> UpdateRegistration(CommonMealRegistration registration)
        {
            var status = await _mealRepository.GetStatus(registration.CommonMealId);

            // Prevent registration if meal is closed (the GUI tries to prevent this - but might fail)
            if (status != "OPEN")
                throw new AppException(AppErrorCodes.MealIsClosed, $"The meal with id '{registration.CommonMealId}' is not open for registration updates");

            var people = (await _personRepository.GetAll()).ToDictionary(x => x.Id, x => x);
            
            // Prevent registration if limit on meal size
            var maxPeople = _commonMealSettings.GetMaxPeople("STANDARD");
            if (maxPeople > 0 && registration.Attending && !registration.IsTakeAway && people[registration.PersonId].IsAdult())
            {
                var registrations = await _registrationRepository.GetByCommonMealId(registration.CommonMealId);
                var othersAttending = registrations.Count(x => x.Attending && x.PersonId != registration.PersonId && !x.IsTakeAway && people[registration.PersonId].IsAdult());
                    
                if (othersAttending >= maxPeople)
                    throw new AppException(AppErrorCodes.MealIsFull, $"The meal is full - maybe sign up for TAKE-AWAY instead?");
            }

            registration.Timestamp = _timeProvider.Now();
            return await _registrationRepository.Update(registration);
        }
    }
}