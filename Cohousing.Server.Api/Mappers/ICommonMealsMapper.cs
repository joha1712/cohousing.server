using System;
using System.Collections.Immutable;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Api.Mappers
{
    public interface ICommonMealsMapper
    {
        CommonMealsViewModel Map(IImmutableList<CommonMeal> meals, DateTime startOfWeekDate);
    }
}