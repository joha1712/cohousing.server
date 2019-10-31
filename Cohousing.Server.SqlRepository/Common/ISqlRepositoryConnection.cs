using System.Data;

namespace Cohousing.Server.SqlRepository
{
    public interface ISqlRepositoryConnectionFactory {
        IDbConnection New();
    }    
}