using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealExpenseController : ControllerBase
    {
        private readonly ICommonMealExpenseRepository _commonMealExpenseRepository;
        private readonly ICommonMealExpenseMapper _commonMealExpenseMapper;

        public CommonMealExpenseController(ICommonMealExpenseRepository commonMealExpenseRepository, ICommonMealExpenseMapper commonMealExpenseMapper)
        {
            _commonMealExpenseRepository = commonMealExpenseRepository;
            _commonMealExpenseMapper = commonMealExpenseMapper;
        }

        // PUT api/values
        [HttpPut("expense")]
        public async Task<ActionResult> UpdateExpense(CommonMealExpenseViewModel expense)
        {
            var expenseModel = _commonMealExpenseMapper.Map(expense);
            
            if (expenseModel.Id == -1)
                await _commonMealExpenseRepository.Add(expenseModel);
            else
                await _commonMealExpenseRepository.Update(expenseModel);
            
            return Ok();
        }
    }
}