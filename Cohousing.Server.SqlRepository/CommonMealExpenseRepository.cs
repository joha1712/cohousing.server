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

        public async Task Update(CommonMealExpense expense)
        {
            const string query =
                " UPDATE commonMealExpense " +
                " SET personId = @PersonId, commonMealId = @MealId, date = @Date, amount = @Amount " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new
                {
                    Id = expense.Id,
                    PersonId = expense.PersonId,
                    MealId = expense.MealId,
                    Date = expense.Timestamp,
                    Amount = expense.Amount
                });
            }
        }
        
        public async Task<CommonMealExpense> Add(CommonMealExpense expense)
        {
            const string query =
                " INSERT INTO commonMealExpense (personId, date, commonMealId, amount) " +
                " VALUES (@PersonId, @Date, @MealId, @Amount) " +
                " RETURNING id ";
           
            using (var connection = _connectionFactory.New())
            {
                var id = await connection.QueryAsync<int>(query, new
                {
                    PersonId = expense.PersonId,
                    MealId = expense.MealId,
                    Date = expense.Timestamp,
                    Amount = expense.Amount
                });

                expense.Id = id.Single();
                return expense;
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