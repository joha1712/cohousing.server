using System.Data.SqlClient;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;

namespace Cohousing.Server.SqlRepository
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public ConfigRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Config GetByKey(string key)
        {
            const string query = " SELECT [Id] Id, [Key] [Key], [Value] [Value] " +
                                 " FROM [Config] " +
                                 " WHERE [Key] = @Key ";

            using (var connection = _connectionFactory.New())
            {
                return connection.QuerySingle<Config>(query, new { Key = key });
            }
        }
    }
}