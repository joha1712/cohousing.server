using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model;

namespace Cohousing.Server.Api.Mappers
{
    public interface ICommonMealShoppingInfoMapper
    {
        CommonMealShoppingInfo Map(CommonMealShoppingInfoViewModel item);
        Task<CommonMealShoppingInfoViewModel> Map(CommonMealShoppingInfo item);
    }
}