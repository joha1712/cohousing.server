using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealShoppingInfoController : ControllerBase
    {
        private readonly ICommonMealShoppingInfoService _commonMealShoppingInfoService;
        private readonly ICommonMealShoppingInfoMapper _commonMealShoppingInfoMapper;

        public CommonMealShoppingInfoController(ICommonMealShoppingInfoService commonMealShoppingInfoService, ICommonMealShoppingInfoMapper commonMealShoppingInfoMapper)
        {
            _commonMealShoppingInfoService = commonMealShoppingInfoService;
            _commonMealShoppingInfoMapper = commonMealShoppingInfoMapper;
        }

        // GET api/values
        [HttpGet("shoppinginfo")]
        public async Task<ActionResult<CommonMealShoppingInfoViewModel>> Get(int mealId)
        {
            var shoppingInfo = await _commonMealShoppingInfoService.Load(mealId);
            var result = await _commonMealShoppingInfoMapper.Map(shoppingInfo);
            return result;
        }
    }
}