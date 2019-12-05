using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;
// ReSharper disable RedundantAnonymousTypePropertyName

namespace Cohousing.Server.SqlRepository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public PersonRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Person> GetById(int id)
        {
            const string query = " SELECT id AS Id, active AS Active, firstName AS FirstName, lastName AS LastName, addressId AS AddressId, callName AS CallName, attributes AS Attributes " +
                                 " FROM person " +
                                 " WHERE id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                return await connection.QuerySingleAsync<Person>(query, new { Id = id });
            }
        }

        public async Task<IImmutableList<Person>> GetAll()
        {
            const string query = " SELECT id AS Id, active AS Active, firstName AS FirstName, lastName AS LastName, addressId AS AddressId, callName As CallName, attributes AS Attributes " +
                                 " FROM person ";

            using (var connection = _connectionFactory.New())
            {
                var result = await connection.QueryAsync<Person>(query);
                return result.ToImmutableList();
            }
        }

        public async Task<Person> Add(Person person)
        {
            const string query =
                " INSERT INTO Person (Active, FirstName, LastName, AddressId) " +
                " OUTPUT Inserted.Id " +
                " VALUES (@Active, @FirstName, @LastName, @AddressId) ";

            using (var connection = _connectionFactory.New())
            {
                var result = await connection.QueryAsync<int>(query, new { Active = person.Active, FirstName = person.FirstName, LastName = person.LastName, AddressId = person.AddressId});
                person.Id = result.SingleOrDefault();

                return person;
            }
        }
    }
}