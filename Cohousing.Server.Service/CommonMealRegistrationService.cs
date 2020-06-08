﻿using System;
using System.Threading.Tasks;
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

        public CommonMealRegistrationService(ICommonMealRegistrationRepository registrationRepository, ICommonMealRepository mealRepository, ITimeProvider timeProvider)
        {
            _registrationRepository = registrationRepository;
            _mealRepository = mealRepository;
            _timeProvider = timeProvider;
        }

        public async Task<CommonMealRegistration> UpdateRegistration(CommonMealRegistration registration)
        {
            var status = await _mealRepository.GetStatus(registration.CommonMealId);

            if (status != "OPEN")
                throw new Exception($"The meal with id '{registration.CommonMealId}' is not open for registration updates");

            registration.Timestamp = _timeProvider.Now();
            return await _registrationRepository.Update(registration);
        }
    }
}