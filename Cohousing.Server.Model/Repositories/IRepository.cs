using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Cohousing.Server.Model.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        Task<IImmutableList<T>> GetAll();
        Task<T> Add(T commonMeal);
    }
}