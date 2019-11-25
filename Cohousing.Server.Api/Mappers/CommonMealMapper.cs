using System;
using System.Linq;
using Cohousing.Server.Api.Common;
using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Util;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealMapper : MapperBase<CommonMeal, CommonMealViewModel>, ICommonMealMapper
    {
        private readonly ITimeFormatter _timeFormatter;
        private readonly ICommonMealRegistrationGroupFactory _registrationGroupFactory;
        private readonly ICommonMealRegistrationMapper _commonMealRegistrationMapper;
        private readonly ICommonMealChefMapper _commonMealChefMapper;
        private readonly ITimeProvider _timeProvider;

        public CommonMealMapper(ITimeFormatter timeFormatter, ICommonMealRegistrationGroupFactory registrationGroupFactory, ICommonMealRegistrationMapper commonMealRegistrationMapper, ICommonMealChefMapper commonMealChefMapper, ITimeProvider timeProvider)
        {
            _timeFormatter = timeFormatter;
            _registrationGroupFactory = registrationGroupFactory;
            _commonMealRegistrationMapper = commonMealRegistrationMapper;
            _commonMealChefMapper = commonMealChefMapper;
            _timeProvider = timeProvider;
        }

        public override CommonMealViewModel Map(CommonMeal item)
        {
            var registrations = _commonMealRegistrationMapper.MapMany(item.Registrations);

            var result = new CommonMealViewModel
            {
                Id = item.Id + "",
                Date = item.Date,
                DateName = _timeFormatter.GetDateName(item.Date),
                DayName = _timeFormatter.GetDayName(item.Date).ToUpperFirstLetter(),
                IsActiveMeal = _timeProvider.Now.Date == item.Date.Date,
                RegistrationGroups = _registrationGroupFactory.CreateGroups(registrations),
                Chefs = _commonMealChefMapper.MapMany(item.Chefs)
            };

            CalcRegistrationNumber(result);

            return result;
        }

        public override CommonMeal Map(CommonMealViewModel item)
        {
            var registrations = item.RegistrationGroups.SelectMany(x => x.Registrations);

            return new CommonMeal
            {
                Id = Convert.ToInt32(item.Id),
                Date = item.Date,
                Registrations = _commonMealRegistrationMapper.MapMany(registrations)
            };
        }

        private static void CalcRegistrationNumber(CommonMealViewModel result)
        {
            var regNo = 1;
            foreach (var reg in result.RegistrationGroups.SelectMany(x => x.Registrations))
            {
                reg.RegNo = regNo++;
            }
        }
    }
}