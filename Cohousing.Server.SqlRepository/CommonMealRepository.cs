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
    public partial class CommonMealRepository : ICommonMealRepository
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

        public async Task<CommonMeal> GetByDate(DateTime date)
        {
            const string query = " SELECT [Id] Id, [Date] Date " +
                                 " FROM CommonMeal " +
                                 " WHERE Date = @Date ";

            using (var connection = _connectionFactory.New())
            {
                var commonMeal = await connection.QuerySingleOrDefaultAsync<CommonMeal>(query, new { Date = date });

                if (commonMeal == null)
                    return null;

                commonMeal.Registrations = await _commonMealRegistrationRepository.GetByCommonMealId(commonMeal.Id);
                commonMeal.Chefs = await _commonMealChefRepository.GetByCommonMealId(commonMeal.Id);
                return commonMeal;
            }
        }

        public async Task<IImmutableList<CommonMeal>> GetPreviousByDate(DateTime date, int numPrevious)
        {
            const string query = " SELECT TOP (@NumPrevious) [Id] Id " +
                                 " FROM CommonMeal " +
                                 " WHERE Date < @Date " +
                                 " ORDER BY Date Desc ";
            int[] ids;

            using (var connection = _connectionFactory.New())
            {
                ids = (await connection.QueryAsync<int>(query, new { Date = date, NumPrevious = numPrevious })).ToArray();
            }

            var result = await Task.WhenAll(ids.Select(GetById));
            return result.ToImmutableList();
        }

        public async Task<CommonMeal> GetById(int id)
        {
            const string query = " SELECT [Id] Id, [Date] Date " +
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

        public async Task<IImmutableList<CommonMeal>> GetAll()
        {
            const string query = " SELECT [Id] Id " +
                                 " FROM CommonMeal ";
            int[] ids;

            using (var connection = _connectionFactory.New())
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

            using (var connection = _connectionFactory.New())
            {
                var output = await connection.QueryAsync<int>(query, new { Date = commonMeal.Date });
                commonMeal.Id = output.SingleOrDefault();

                // Add common meal registrations
                commonMeal.Registrations = await _commonMealRegistrationRepository.AddMany(commonMeal.Registrations, commonMeal.Id);
                commonMeal.Chefs = await _commonMealChefRepository.AddMany(commonMeal.Chefs, commonMeal.Id);

                return commonMeal;
            }
        }
    }
}