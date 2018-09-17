﻿using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealRepository : IRepository<CommonMeal>
    {
        Task<CommonMeal> GetByDate(DateTime date);
        Task<IImmutableList<CommonMeal>> GetPreviousByDate(DateTime date, int numPrevious);
    }
}