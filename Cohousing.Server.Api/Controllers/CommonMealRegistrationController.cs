using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealRegistrationController : ControllerBase
    {
        private readonly ICommonMealRegistrationMapper _commonMealRegistrationMapper;
        private readonly ICommonMealRegistrationService _commonMealRegistrationService;

        public CommonMealRegistrationController(ICommonMealRegistrationMapper commonMealRegistrationMapper, ICommonMealRegistrationService commonMealRegistrationService)
        {
            _commonMealRegistrationMapper = commonMealRegistrationMapper;
            _commonMealRegistrationService = commonMealRegistrationService;
        }

        // PUT api/values
        [HttpPut("registrations")]
        public async Task<CommonMealRegistrationViewModel> UpdateRegistration(CommonMealRegistrationViewModel registration)
        {
            var reg = _commonMealRegistrationMapper.Map(registration);
            var result = await _commonMealRegistrationService.UpdateRegistration(reg);

            return _commonMealRegistrationMapper.Map(result);
        }
    }
}