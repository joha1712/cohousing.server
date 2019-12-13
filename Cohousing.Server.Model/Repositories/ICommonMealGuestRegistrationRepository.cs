using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealGuestRegistrationRepository
    {
        Task<CommonMealGuestRegistration> GetById(int id);
        Task<CommonMealGuestRegistration> GetByRegistrationId(int id);  
        Task<CommonMealGuestRegistration> Update(CommonMealGuestRegistration reg);
        Task<CommonMealGuestRegistration> Add(CommonMealGuestRegistration reg);
    }
}