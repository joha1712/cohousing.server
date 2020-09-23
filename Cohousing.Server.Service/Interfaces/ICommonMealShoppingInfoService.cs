using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Service
{
    public interface ICommonMealShoppingInfoService
    {
        Task<CommonMealShoppingInfo> Load(int mealId);
    }
}