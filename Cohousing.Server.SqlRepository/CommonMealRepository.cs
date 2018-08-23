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
        private readonly ICommonMealRegistrationRepository _commonMealRegistrationRepository;

        public CommonMealRepository(ISqlRepositorySettings settings, ICommonMealRegistrationRepository commonMealRegistrationRepository)
        {
            _settings = settings;
            _commonMealRegistrationRepository = commonMealRegistrationRepository;
        }

        public async Task<CommonMeal> GetByDate(DateTime date)
        {
            const string query = " SELECT [Id] Id, [Date] Date " +
                                 " FROM CommonMeal " +
                                 " WHERE Date = @Date ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var commonMeal = await connection.QuerySingleOrDefaultAsync<CommonMeal>(query, new { Date = date });

                if (commonMeal == null)
                    return null;

                commonMeal.Registrations = await _commonMealRegistrationRepository.GetByCommonMealId(commonMeal.Id);
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
                var registrations = await _commonMealRegistrationRepository.GetByCommonMealId(commonMeal.Id);

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
                commonMeal.Registrations = await _commonMealRegistrationRepository.Add(commonMeal.Registrations, commonMeal.Id);

                return commonMeal;
            }
        }

        
    }
}