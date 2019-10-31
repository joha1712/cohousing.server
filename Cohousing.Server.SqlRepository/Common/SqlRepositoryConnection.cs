using System.Data;
using Npgsql;

namespace Cohousing.Server.SqlRepository
{
    public class SqlRepositoryConnectionFactory : ISqlRepositoryConnectionFactory {
        private readonly string _dbConnectionString;

        public SqlRepositoryConnectionFactory(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public IDbConnection New() {
            return new NpgsqlConnection(_dbConnectionString);
        }        
    }
}