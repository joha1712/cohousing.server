using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealChefController : ControllerBase
    {
        private readonly ICommonMealChefMapper _commonMealChefMapper;
        private readonly ICommonMealChefRepository _commonMealChefRepository;
        private readonly ITimeProvider _timeProvider;

        public CommonMealChefController(ICommonMealChefMapper commonMealChefMapper, ICommonMealChefRepository commonMealChefRepository, ITimeProvider timeProvider)
        {
            _commonMealChefMapper = commonMealChefMapper;
            _commonMealChefRepository = commonMealChefRepository;
            _timeProvider = timeProvider;
        }

        // PUT api/values
        [HttpPut("chefs")]
        public async Task<ActionResult> UpdateChef(CommonMealChefViewModel chef)
        {
            var chefModel = _commonMealChefMapper.Map(chef);
            chefModel.Timestamp = _timeProvider.Now();

            await _commonMealChefRepository.Update(chefModel);
            return Ok();
        }
    }
}