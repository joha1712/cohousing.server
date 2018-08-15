﻿using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;

namespace Cohousing.Server.SqlRepository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ISqlRepositorySettings _settings;

        public PersonRepository(ISqlRepositorySettings settings)
        {
            _settings = settings;
        }

        public async Task<Person> GetById(int id)
        {
            const string query = " SELECT [Id] Id, [Active] Active, [FirstName] FirstName, [LastName] LastName " +
                                 " FROM Person " +
                                 " WHERE Id = @Id ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                return await connection.QuerySingleAsync<Person>(query, new { Id = id });
            }
        }

        public async Task<IImmutableList<Person>> GetAll()
        {
            const string query = " SELECT [Id] Id, [Active] Active, [FirstName] FirstName, [LastName] LastName " +
                                 " FROM Person ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var result = await connection.QueryAsync<Person>(query);
                return result.ToImmutableList();
            }
        }

        public async Task<Person> Add(Person commonMeal)
        {
            const string query = 
                " INSERT INTO Person ([Active], [FirstName], [LastName]) " +
                " OUTPUT Inserted.Id " +
                " VALUES (@Active, @FirstName, @LastName) ";

            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                var result = await connection.QueryAsync<int>(query, new { Active = commonMeal.Active, FirstName = commonMeal.FirstName, LastName = commonMeal.LastName});
                commonMeal.Id = result.SingleOrDefault();

                return commonMeal;
            }
        }
    }
}