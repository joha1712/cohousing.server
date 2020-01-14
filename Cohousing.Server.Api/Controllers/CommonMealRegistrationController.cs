using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealRegistrationController : ControllerBase
    {
        private readonly ICommonMealRegistrationMapper _commonMealRegistrationMapper;
        private readonly ICommonMealRegistrationRepository _commonMealRegistrationRepository;

        public CommonMealRegistrationController(ICommonMealRegistrationMapper commonMealRegistrationMapper, ICommonMealRegistrationRepository commonMealRegistrationRepository)
        {
            _commonMealRegistrationMapper = commonMealRegistrationMapper;
            _commonMealRegistrationRepository = commonMealRegistrationRepository;
        }

        // PUT api/values
        [HttpPut("registrations")]
        public async Task<CommonMealRegistrationViewModel> UpdateRegistration(CommonMealRegistrationViewModel registration)
        {
            var reg = _commonMealRegistrationMapper.Map(registration);
            var result = await _commonMealRegistrationRepository.Update(reg);

            return _commonMealRegistrationMapper.Map(result);
        }
    }
}