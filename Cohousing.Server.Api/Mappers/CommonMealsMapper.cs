﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealsMapper : ICommonMealsMapper
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICommonMealMapper _commonMealMapper;
        
        public CommonMealsMapper(IPersonRepository personRepository, ICommonMealMapper commonMealMapper)
        {
            _personRepository = personRepository;
            _commonMealMapper = commonMealMapper;
        }

        public async Task<CommonMealsViewModel> Map(IImmutableList<CommonMeal> meals, DateTime startOfWeekDate)
        {
            var people = await  _personRepository.GetAll();
            var peopleSorted = people.OrderBy(p => p.IsAdult() ? "First" : "Last").ThenBy(x => x.CallName).ToImmutableList();
            var weekNo = startOfWeekDate.GetIso8601WeekNo().ToString() ?? "{ukendt}";
            var mappedMeals = _commonMealMapper
                .MapMany(meals)
                .OrderBy(x => x.Date)
                .ToImmutableList();

            return new CommonMealsViewModel
            {
                Title = $"Madplan - uge {weekNo}",
                Subtitle = "Buske bofællesskab",
                Meals = mappedMeals,
                WeekDate = startOfWeekDate,
                People = peopleSorted.Select(x => new PersonViewModel
                {
                    Id = x.Id,
                    Name = x.CallName
                }).ToImmutableList()
            };
        }
    }
}