using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealGuestRegistrationService
    {
         Task<CommonMealGuestRegistration> LoadOrCreateDefault(int regId);
         Task<CommonMealGuestRegistration> Load(int regId);
         CommonMealGuestRegistration CreateDefault(int regId);

         Task<CommonMealGuestRegistration> Save(CommonMealGuestRegistration reg);
    }
}