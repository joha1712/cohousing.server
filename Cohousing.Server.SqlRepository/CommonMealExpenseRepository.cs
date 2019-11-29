using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;
// ReSharper disable RedundantAnonymousTypePropertyName
#pragma warning disable IDE0037

namespace Cohousing.Server.SqlRepository
{
    public class CommonMealExpenseRepository : ICommonMealExpenseRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public CommonMealExpenseRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CommonMealExpense> GetById(int id)
        {
            const string query = " SELECT id As Id, personId AS PersonId, commonMealId AS MealId, date AS Timestamp, amount AS Amount " +
                                 " FROM commonMealExpense " +
                                 " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                return await connection.QuerySingleOrDefaultAsync<CommonMealExpense>(query, new { Id = id });
            }
        }


        public async Task<IImmutableList<CommonMealExpense>> GetByCommonMealId(int commonMealId)
        {
            const string query = " SELECT id As Id, personId AS PersonId, commonMealId AS MealId, date AS Timestamp, amount AS Amount " +
                                 " FROM commonMealExpense " +
                                 " WHERE commonMealId = @CommonMealId ";

            using (var connection = _connectionFactory.New())
            {
                var result = await connection.QueryAsync<CommonMealExpense>(query, new { CommonMealId = commonMealId });
                return result.ToImmutableList();
            }
        }
    }
}