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

        public CommonMealRegistrationService(ICommonMealRegistrationRepository registrationRepository, ICommonMealRepository mealRepository, ITimeProvider timeProvider, ICommonMealSettings commonMealSettings)
        {
            _registrationRepository = registrationRepository;
            _mealRepository = mealRepository;
            _timeProvider = timeProvider;
            _commonMealSettings = commonMealSettings;
        }

        public async Task<CommonMealRegistration> UpdateRegistration(CommonMealRegistration registration)
        {
            var status = await _mealRepository.GetStatus(registration.CommonMealId);

            // Prevent registration if meal is closed (the GUI tries to prevent this - but might fail)
            if (status != "OPEN")
                throw new AppException(AppErrorCodes.MealIsClosed, $"The meal with id '{registration.CommonMealId}' is not open for registration updates");

            // Prevent registration if limit on meal size
            var maxPeopleStandard = _commonMealSettings.GetMaxPeople("STANDARD");
            var maxTakeAway = _commonMealSettings.GetMaxPeople("TAKEAWAY");

            if (registration.Attending && (maxPeopleStandard > 0 || maxTakeAway > 0))
            {
                var registrations = await _registrationRepository.GetByCommonMealId(registration.CommonMealId);
                var othersAttendingStandard = registrations.Count(x => x.Attending && x.PersonId != registration.PersonId && !x.IsTakeAway);
                var othersAttendingTakeAway = registrations.Count(x => x.Attending && x.PersonId != registration.PersonId && x.IsTakeAway);
                
                if (!registration.IsTakeAway && othersAttendingStandard >= maxPeopleStandard)
                    throw new AppException(AppErrorCodes.MealIsFull, $"The meal is full - maybe sign up for TAKE-AWAY instead?");
                
                if (registration.IsTakeAway && othersAttendingTakeAway >= maxTakeAway)
                    throw new AppException(AppErrorCodes.TakeAwayMealIsFull, $"The meal is full - maybe sign up for STANDARD instead?");
            }

            registration.Timestamp = _timeProvider.Now();
            return await _registrationRepository.Update(registration);
        }
    }
}