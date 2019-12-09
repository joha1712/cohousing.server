using System.Threading.Tasks;
using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cohousing.Server.Api.Controllers
{
    [Route("api/commonmeals/guestregistrations")]
    [ApiController]
    public class CommonMealGuestRegistrationsController : ControllerBase
    {
        private readonly ICommonMealRegistrationMapper _commonMealRegistrationMapper;
        private readonly ICommonMealRegistrationRepository _commonMealRegistrationRepository;

        public CommonMealGuestRegistrationsController(ICommonMealRegistrationMapper commonMealRegistrationMapper, ICommonMealRegistrationRepository commonMealRegistrationRepository)
        {
            _commonMealRegistrationMapper = commonMealRegistrationMapper;
            _commonMealRegistrationRepository = commonMealRegistrationRepository;
        }

        // PUT api/values
        [HttpGet("list")]
        public ActionResult<CommonMealGuestRegistrationsViewModel> ListGuests(int registrationId)
        {
            return new CommonMealGuestRegistrationsViewModel {
                Id = -1,
                RegistrationId = registrationId,                
                Adults = new PersonGroupViewModel {
                    Conventional = 0,
                    Vegetarians = 0,
                },
                Children = new PersonGroupViewModel {
                    Conventional = 0,
                    Vegetarians = 0
                }
            };
        }
    }

    public class CommonMealGuestRegistrationsViewModel {
        public int Id { get; set;}
        public int RegistrationId { get; set;}
        public PersonGroupViewModel Adults { get; set;}
        public PersonGroupViewModel Children { get; set;}
    }
}