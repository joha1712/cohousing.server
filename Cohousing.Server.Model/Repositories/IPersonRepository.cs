using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<IImmutableList<Person>> GetAll();
    }
}
