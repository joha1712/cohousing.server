using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealGuestRegistrationMapper : MapperBase<CommonMealGuestRegistration, CommonMealGuestRegistrationViewModel>, ICommonMealGuestRegistrationMapper
    {
        private readonly IPersonGroupMapper _personGroupMapper;

        public CommonMealGuestRegistrationMapper(IPersonGroupMapper personGroupMapper) {
            _personGroupMapper = personGroupMapper;
        }

        public override CommonMealGuestRegistration Map(CommonMealGuestRegistrationViewModel item)
        {
            return new CommonMealGuestRegistration {
                RegistrationId = item.RegistrationId,
                Adults = _personGroupMapper.Map(item.Adults),
                Children = _personGroupMapper.Map(item.Children)
            };
        }

        public override CommonMealGuestRegistrationViewModel Map(CommonMealGuestRegistration item)
        {
            return new CommonMealGuestRegistrationViewModel {
                RegistrationId = item.RegistrationId,
                Adults = _personGroupMapper.Map(item.Adults),
                Children = _personGroupMapper.Map(item.Children)
            };
        }
    }
}