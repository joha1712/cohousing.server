using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface ICommonMealChefRepository
    {
        Task<IImmutableList<CommonMealChef>> GetByCommonMealId(int commonMealId);
        Task<CommonMealChef> GetById(int id);
        Task<IImmutableList<CommonMealChef>> GetAll();

        Task<IImmutableList<CommonMealChef>> AddMany(IImmutableList<CommonMealChef> chefs);
        Task<CommonMealChef> Add(CommonMealChef chef);
        Task<CommonMealChef> Update(CommonMealChef chef);
    }
}