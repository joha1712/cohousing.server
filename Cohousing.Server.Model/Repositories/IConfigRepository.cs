using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Repositories
{
    public interface IConfigRepository
    {
        Config GetByKey(string key);
    }
}