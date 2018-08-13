using Cohousing.Server.Model.Models;
using Cohousing.Server.RestApi.Mappers.Common;
using Cohousing.Server.RestApi.ViewModels;

namespace Cohousing.Server.RestApi.Mappers
{
    public interface ICommonMealMapper : IMapper<CommonMeal, CommonMealViewModel>
    {
    }
}