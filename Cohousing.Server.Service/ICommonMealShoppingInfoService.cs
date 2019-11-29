using System.Threading.Tasks;

namespace Cohousing.Server.Service
{
    public interface ICommonMealShoppingInfoService
    {
        Task<object> Load(int mealId);
    }
}