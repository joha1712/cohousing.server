using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;
// ReSharper disable RedundantAnonymousTypePropertyName
#pragma warning disable IDE0037

namespace Cohousing.Server.SqlRepository
{
    public class CommonMealChefRepository : ICommonMealChefRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public CommonMealChefRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CommonMealChef> GetById(int id)
        {
            const string query = " SELECT id As Id, personId AS PersonId, date AS Timestamp " +
                                 " FROM CommonMealChef " +
                                 " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                return await connection.QuerySingleOrDefaultAsync<CommonMealChef>(query, new { Id = id });
            }
        }

        public async Task<IImmutableList<CommonMealChef>> GetAll()
        {
            const string query = " SELECT id AS Id, personId AS PersonId, date AS Timestamp " +
                                 " FROM CommonMealChef ";

            using (var connection = _connectionFactory.New())
            {
                var result = await connection.QueryAsync<CommonMealChef>(query);
                return result.ToImmutableList();
            }
        }

        public async Task<IImmutableList<CommonMealChef>> AddMany(IImmutableList<CommonMealChef> chefs, int commonMealId)
        {
            foreach (var chef in chefs)
            {
                await Add(chef, commonMealId);
            }

            return chefs;
        }

        public async Task<CommonMealChef> Add(CommonMealChef commonMealChef, int commonMealId)
        {
            const string query =
                " INSERT INTO CommonMealChef (personId, date, commonMealId) " +
                " VALUES (@PersonId, @Date, @CommonMealId) " +
                " RETURNING id ";

            using (var connection = _connectionFactory.New())
            {
                var id = await connection.QueryAsync<int>(query, new
                {
                    PersonId = commonMealChef.PersonId,
                    Date = commonMealChef.Timestamp,
                    CommonMealId = commonMealId
                });

                commonMealChef.Id = id.Single();
                return commonMealChef;
            }
        }

        public async Task<IImmutableList<CommonMealChef>> GetByCommonMealId(int commonMealId)
        {
            const string query = " SELECT id AS id, personId AS personId, date AS Timestamp " +
                                 " FROM CommonMealChef " +
                                 " WHERE CommonMealId = @CommonMealId ";

            using (var connection = _connectionFactory.New())
            {
                var result = await connection.QueryAsync<CommonMealChef>(query, new { CommonMealId = commonMealId });
                return result.ToImmutableList();
            }
        }

        public async Task<CommonMealChef> Update(CommonMealChef commonMealChef)
        {
            const string query =
                " UPDATE CommonMealChef " +
                " SET PersonId = @PersonId, date = @Date " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new
                {
                    Id = commonMealChef.Id,
                    PersonId = commonMealChef.PersonId,
                    Date = commonMealChef.Timestamp
                });

                return commonMealChef;
            }
        }
    }
}