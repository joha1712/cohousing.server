using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealRegistrationRepository
    {
        Task<CommonMealRegistration> GetById(int id);

        Task<IImmutableList<CommonMealRegistration>> GetAll();

        Task<IImmutableList<CommonMealRegistration>> GetByCommonMealId(int commonMealId);

        Task<IImmutableList<CommonMealRegistration>> AddMany(IImmutableList<CommonMealRegistration> registrations, int commonMealId);

        Task<CommonMealRegistration> Update(CommonMealRegistration registration);
    }
}