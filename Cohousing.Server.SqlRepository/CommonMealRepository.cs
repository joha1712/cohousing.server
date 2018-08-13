using System;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;
// ReSharper disable RedundantAnonymousTypePropertyName

namespace Cohousing.Server.SqlRepository
{
    public class CommonMealRepository : ICommonMealRepository
    {
        private readonly ISqlRepositorySettings _settings;
        
        public CommonMealRepository(ISqlRepositorySettings settings)
        {
            _settings = settings;
        }

        public async Task<CommonMeal> GetByDate(DateTime date)
        {
            const string query = " SELECT [Id] Id, [Date] Date " +
                                 " FROM CommonMeal " +
                                 " WHERE Date = @Date ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var commonMeal = await connection.QuerySingleAsync<CommonMeal>(query, new { Date = date });
                var registrations = await GetCommonMealRegistrations(commonMeal.Id);

                commonMeal.Registrations = registrations;
                return commonMeal;
            }
        }
        
        public async Task<CommonMeal> GetById(int id)
        {
            const string query = " SELECT [Id] Id, [Date] Date " +
                                 " FROM CommonMeal " +
                                 " WHERE Id = @Id ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var commonMeal = await connection.QuerySingleAsync<CommonMeal>(query, new { Id = id });
                var registrations = await GetCommonMealRegistrations(commonMeal.Id);

                commonMeal.Registrations = registrations;
                return commonMeal;
            }
        }

        public async Task<IImmutableList<CommonMeal>> GetAll()
        {
            const string query = " SELECT [Id] Id " +
                                 " FROM CommonMeal ";
            int[] ids;

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                ids = 
                    (await connection.QueryAsync<int>(query))
                    .ToArray();
            }

            var result = await Task.WhenAll(ids.Select(GetById));
            return result.ToImmutableList();
        }

        public async Task<CommonMeal> Add(CommonMeal commonMeal)
        {
            const string query = 
                " INSERT INTO CommonMeal ([Date]) " +
                " OUTPUT Inserted.Id " +
                " VALUES (@Date) ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var output = await connection.QueryAsync<int>(query, new { Date = commonMeal.Date });
                commonMeal.Id = output.SingleOrDefault();

                // Add common meal registrations
                commonMeal.Registrations = await AddCommonMealRegistrations(commonMeal.Registrations, commonMeal.Id);

                return commonMeal;
            }
        }

        private async Task<IImmutableList<CommonMealRegistration>> AddCommonMealRegistrations(IImmutableList<CommonMealRegistration> registrations, int commonMealId)
        {
            const string query =
                " INSERT INTO CommonMealRegistration ([PersonId], [CommonMealId], [Attending]) " +
                " OUTPUT Inserted.Id " +
                " VALUES (@PersonId, @CommonMealId, @Attending) ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var ids = (await connection.QueryAsync<int>(query, registrations.Select(x => new
                {
                    PersonId = x.PersonId,
                    CommonMealId = commonMealId,
                    Attending = x.Attending
                })))
                .ToArray();

                for (var i = 0; i < registrations.Count; i++)
                {
                    registrations[i].Id = ids[i];
                }

                return registrations;
            }
        }

        private async Task<IImmutableList<CommonMealRegistration>> GetCommonMealRegistrations(int commonMealId)
        {
            const string query = " SELECT [Id] Id, [Attending] Attending, [PersonId] PersonId " +
                                 " FROM CommonMealRegistration " +
                                 " WHERE CommonMealId = @CommonMealId ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var registrations = await connection.QueryAsync<CommonMealRegistration>(query, new {CommonMealId = commonMealId});

                return registrations.ToImmutableList();
            }
        }
    }
}