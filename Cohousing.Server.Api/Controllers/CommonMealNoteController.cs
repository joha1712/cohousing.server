using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals")]
    [ApiController]
    public class CommonMealNoteController : ControllerBase
    {
        private readonly ICommonMealRepository _commonMealRepository;

        public CommonMealNoteController(ICommonMealRepository commonMealRepository)
        {
            _commonMealRepository = commonMealRepository;
        }

        // PUT api/values
        [HttpPut("note")]
        public async Task<ActionResult> UpdateNote(CommonMealNoteViewModel note)
        {
            await _commonMealRepository.UpdateNote(note.Id, note.Note?.Trim());
            return Ok();
        }
    }
}