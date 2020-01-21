using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealRegistrationService
    {
        Task<CommonMealRegistration> UpdateRegistration(CommonMealRegistration registration);
    }
}