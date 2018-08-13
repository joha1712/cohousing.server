﻿using System;
using Cohousing.Server.Model.Models;
using Cohousing.Server.RestApi.Common;
using Cohousing.Server.RestApi.Mappers.Common;
using Cohousing.Server.RestApi.ViewModels;

namespace Cohousing.Server.RestApi.Mappers
{
    public class CommonMealMapper : MapperBase<CommonMeal, CommonMealViewModel>, ICommonMealMapper
    {
        private readonly ITimeFormatter _timeFormatter;
        private readonly IMapper<CommonMealRegistration, CommonMealRegistrationViewModel> _registrationMapper;

        public CommonMealMapper(ITimeFormatter timeFormatter, IMapper<CommonMealRegistration, CommonMealRegistrationViewModel> registrationMapper)
        {
            _timeFormatter = timeFormatter;
            _registrationMapper = registrationMapper;
        }

        public override CommonMealViewModel Map(CommonMeal item)
        {
            return new CommonMealViewModel
            {
                Id = item.Id + "",
                Date = item.Date,
                DateName = _timeFormatter.GetDateName(item.Date),
                DayName = _timeFormatter.GetDayName(item.Date),
                Registrations = _registrationMapper.Map(item.Registrations)
            };
        }
        
        public override CommonMeal Map(CommonMealViewModel item)
        {
            return new CommonMeal
            {
                Id = Convert.ToInt32(item.Id),
                Date = item.Date,
                Registrations = _registrationMapper.Map(item.Registrations)
            };
        }
    }
}