using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Service
{
    public interface ICommonMealChefService
    {
        Task<CommonMealChef> UpdateChef(CommonMealChef chef);
    }
}