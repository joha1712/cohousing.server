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
    public class CommonMealRegistrationRepository : ICommonMealRegistrationRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public CommonMealRegistrationRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CommonMealRegistration> GetById(int id)
        {
            const string query = " SELECT [Id] Id, [Attending] Attending, [PersonId] PersonId, [CommonMealId] CommonMealId " +
                                 " FROM CommonMealRegistration " +
                                 " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = await connection.QueryAsync<CommonMealRegistration>(query, new {Id = id});
                return registrations.SingleOrDefault();
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> GetByCommonMealId(int commonMealId)
        {
            const string query = " SELECT [Id] Id, [Attending] Attending, [PersonId] PersonId, [CommonMealId] CommonMealId " +
                                 " FROM CommonMealRegistration " +
                                 " WHERE CommonMealId = @CommonMealId ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = await connection.QueryAsync<CommonMealRegistration>(query, new {CommonMealId = commonMealId});
                return registrations.ToImmutableList();
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> GetAll()
        {
            const string query = " SELECT [Id] Id, [Attending] Attending, [PersonId] PersonId, [CommonMealId] CommonMealId " +
                                 " FROM CommonMealRegistration ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = await connection.QueryAsync<CommonMealRegistration>(query);
                return registrations.ToImmutableList();
            }
        }

        public async Task<CommonMealRegistration> Add(CommonMealRegistration registration, int commonMealId)
        {
            const string query =
                " INSERT INTO CommonMealRegistration ([PersonId], [CommonMealId], [Attending]) " +
                " OUTPUT Inserted.Id " +
                " VALUES (@PersonId, @CommonMealId, @Attending) ";

            using (var connection = _connectionFactory.New())
            {
                var id = await connection.QueryAsync<int>(query, new
                {
                    PersonId = registration.PersonId,
                    Attending = registration.Attending,
                    CommonMealId = commonMealId
                });

                registration.Id = id.Single();
                return registration;
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> AddMany(IImmutableList<CommonMealRegistration> registrations, int commonMealId)
        {
            foreach (var registration in registrations)
            {
                await Add(registration, commonMealId);
            }

            return registrations;
        }

        public async Task<CommonMealRegistration> Update(CommonMealRegistration registration)
        {
            const string query =
                " UPDATE CommonMealRegistration " +
                " SET [PersonId] = @PersonId, [Attending] = @Attending " +
                " WHERE [Id] = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new
                {
                    Id = registration.Id,
                    PersonId = registration.PersonId,
                    Attending = registration.Attending
                });

                return registration;
            }
        }
    }
}