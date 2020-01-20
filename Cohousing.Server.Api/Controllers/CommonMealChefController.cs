using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealChefController : ControllerBase
    {
        private readonly ICommonMealChefMapper _commonMealChefMapper;
        private readonly ICommonMealChefService _commonMealChefService;        

        public CommonMealChefController(ICommonMealChefMapper commonMealChefMapper, ICommonMealChefService commonMealChefService)
        {
            _commonMealChefMapper = commonMealChefMapper;
            _commonMealChefService = commonMealChefService;            
        }

        // PUT api/values
        [HttpPut("chefs")]
        public async Task<ActionResult> UpdateChef(CommonMealChefViewModel chef)
        {
            var chefModel = _commonMealChefMapper.Map(chef);
            await _commonMealChefService.UpdateChef(chefModel);
            return Ok();
        }
    }
}