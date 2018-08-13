using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealRepository : IRepository<CommonMeal>
    {
        Task<CommonMeal> GetByDate(DateTime date);
    }

    public interface IComonMealRegistrationRepository : IRepository<CommonMealRegistration>
    {
        Task<IList<CommonMealRegistration>> GetByCommonMealId(int id);
    }
}