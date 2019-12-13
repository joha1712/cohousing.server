using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Service
{
    public class CommonMealGuestRegistrationService : ICommonMealGuestRegistrationService
    {
        private readonly ICommonMealGuestRegistrationRepository _commonMealGuestRegistrationRepository;

        public CommonMealGuestRegistrationService(ICommonMealGuestRegistrationRepository commonMealGuestRegistrationRepository) {
            _commonMealGuestRegistrationRepository = commonMealGuestRegistrationRepository;
        }

        public async Task<CommonMealGuestRegistration> LoadOrCreateDefault(int regId) {
            var result = (await Load(regId)) ?? CreateDefault(regId);
            return result;            
        }

        public async Task<CommonMealGuestRegistration> Load(int regId) {
            return await _commonMealGuestRegistrationRepository.GetByRegistrationId(regId);
        }

        public CommonMealGuestRegistration CreateDefault(int regId) {
            return new CommonMealGuestRegistration {
                Id = -1,
                RegistrationId = regId,
                Adults = new PersonGroup(),
                Children = new PersonGroup()
            };
        }

        public async Task<CommonMealGuestRegistration> Save(CommonMealGuestRegistration reg)
        {
            if (reg.Id <= -1)
                return await _commonMealGuestRegistrationRepository.Add(reg);

            return await _commonMealGuestRegistrationRepository.Update(reg);
        }
    }
}