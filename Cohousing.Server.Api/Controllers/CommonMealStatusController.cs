using System;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealStatusController : ControllerBase
    {
        private readonly ICommonMealRepository _commonMealRepository;

        public CommonMealStatusController(ICommonMealRepository commonMealRepository)
        {
            _commonMealRepository = commonMealRepository;
        }

        // PUT api/values
        [HttpPut("status")]
        public async Task<ActionResult> UpdateStatus(CommonMealStatusViewModel status)
        {
            await _commonMealRepository.UpdateStatus(Convert.ToInt32(status.Id), status.Status.Trim().ToUpperInvariant());
            return Ok();
        }
    }
}