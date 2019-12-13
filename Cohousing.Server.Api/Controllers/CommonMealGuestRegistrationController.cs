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
    public class CommonMealGuestRegistrationController : ControllerBase
    {
        private readonly ICommonMealGuestRegistrationService _guestRegistrationService;
        private readonly ICommonMealGuestRegistrationMapper _guestRegistrationMapper;

        public CommonMealGuestRegistrationController(ICommonMealGuestRegistrationService guestRegistrationService, ICommonMealGuestRegistrationMapper commonMealGuestRegistrationMapper)
        {           
            _guestRegistrationService = guestRegistrationService;
            _guestRegistrationMapper = commonMealGuestRegistrationMapper;
        }

        // PUT api/values
        [HttpGet("guestregistration")]
        public async Task<ActionResult<CommonMealGuestRegistrationViewModel>> GetByRegistrationId(int regId)
        {                  
            var guestRegistration = await _guestRegistrationService.LoadOrCreateDefault(regId);
            var result = _guestRegistrationMapper.Map(guestRegistration);            
            return result;
        }

        [HttpPut("guestregistration")]
        public async Task<ActionResult<CommonMealGuestRegistrationViewModel>> SaveRegistration(CommonMealGuestRegistrationViewModel reg)
        {
            var model = _guestRegistrationMapper.Map(reg);
            var result = await _guestRegistrationService.Save(model);
            return  _guestRegistrationMapper.Map(result);
        }
    }    
}