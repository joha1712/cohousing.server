using System;
using System.Collections.Immutable;
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
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;
        private readonly ICommonMealRegistrationRepository _commonMealRegistrationRepository;
        private readonly ICommonMealChefRepository _commonMealChefRepository;

        public CommonMealRepository(ISqlRepositoryConnectionFactory connectionFactory, ICommonMealRegistrationRepository commonMealRegistrationRepository, ICommonMealChefRepository commonMealChefRepository)
        {
            _connectionFactory = connectionFactory;
            _commonMealRegistrationRepository = commonMealRegistrationRepository;
            _commonMealChefRepository = commonMealChefRepository;
        }
        
        public async Task<IImmutableList<CommonMeal>> GetByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            const string query = " SELECT Id As Id, Date AS Date, Note As Note, Status As Status " +
                                 " FROM CommonMeal " +
                                 " WHERE Date >= @DateFrom AND Date <= @DateTo ";

            using (var connection = _connectionFactory.New())
            {
                var commonMeals = (await connection.QueryAsync<CommonMeal>(query, new { DateFrom = dateFrom, DateTo = dateTo })).ToImmutableList();

                foreach (var commonMeal in commonMeals)
                {
                    commonMeal.Registrations = await _commonMealRegistrationRepository.GetByCommonMealId(commonMeal.Id);
                    commonMeal.Chefs = await _commonMealChefRepository.GetByCommonMealId(commonMeal.Id);
                }

                return commonMeals;
            }
        }

        public async Task<CommonMeal> GetByDate(DateTime date)
        {
            return (await GetByDateRange(date, date)).SingleOrDefault();
        }

        public async Task<CommonMeal> GetById(int id)
        {
            const string query = " SELECT Id As Id, Date AS Date, Note As Note, Status As Status " +
                                 " FROM CommonMeal " +
                                 " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                var commonMeal = await connection.QuerySingleAsync<CommonMeal>(query, new { Id = id });
                var registrations = await _commonMealRegistrationRepository.GetByCommonMealId(commonMeal.Id);
                var chefs = await _commonMealChefRepository.GetByCommonMealId(commonMeal.Id);

                commonMeal.Registrations = registrations;
                commonMeal.Chefs = chefs;
                return commonMeal;
            }
        }

        public async Task<CommonMeal> Add(CommonMeal commonMeal) 
        {
            const string query = 
                " INSERT INTO CommonMeal (Date, Note, Status) " +
                " VALUES (@Date, @Note, @Status) " +
                " RETURNING id ";

            using (var connection = _connectionFactory.New())
            {
                var output = await connection.QueryAsync<int>(query, new { Date = commonMeal.Date, Note = commonMeal.Note });
                commonMeal.Id = output.SingleOrDefault();

                // Add common meal registrations
                commonMeal.Registrations = await _commonMealRegistrationRepository.AddMany(commonMeal.Registrations);
                commonMeal.Chefs = await _commonMealChefRepository.AddMany(commonMeal.Chefs, commonMeal.Id);

                return commonMeal;
            }
        }
        
        public async Task UpdateNote(int id, string note)
        {
            const string query = 
                " UPDATE CommonMeal " +
                " SET Note = @Note " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new { Id = id, Note = note });
            }
        }
        
        public async Task UpdateStatus(int id, string status)
        {
            const string query = 
                " UPDATE CommonMeal " +
                " SET Status = @Status " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new { Id = id, Status = status });
            }
        }
        
        public async Task<string> GetStatus(int id)
        {
            const string query = " SELECT Status As Status " +
                                 " FROM CommonMeal " +
                                 " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                var status = await connection.ExecuteScalarAsync<string>(query, new { Id = id });
                return status;
            }
        }
    }
}