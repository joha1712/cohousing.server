using System.Data.SqlClient;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;

namespace Cohousing.Server.SqlRepository
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly string _connectionString;

        public ConfigRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Config GetByKey(string key)
        {
            const string query = " SELECT [Id] Id, [Key] [Key], [Value] [Value] " +
                                 " FROM [Config] " +
                                 " WHERE [Key] = @Key ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Config>(query, new { Key = key });
            }
        }
    }
}