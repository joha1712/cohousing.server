using System;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Dapper;
using Cohousing.Server.Util;
using System.Text;
// ReSharper disable RedundantAnonymousTypePropertyName

namespace Cohousing.Server.SqlRepository
{
    public class CommonMealGuestRegistrationRepository : ICommonMealGuestRegistrationRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;

        public CommonMealGuestRegistrationRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CommonMealGuestRegistration> GetById(int id)
        {
            const string query = " SELECT id As Id, registrationId AS RegistrationId, data AS Data " +
                                 " FROM commonMealGuestRegistration " +
                                 " WHERE id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = await connection.QueryAsync<CommonMealGuestRegistrationDTO>(query, new {Id = id});
                return Map(registrations.SingleOrDefault());
            }
        }

        public async Task<CommonMealGuestRegistration> GetByRegistrationId(int registrationId)
        {
            const string query = " SELECT id As Id, registrationId AS RegistrationId, Data AS Data " +
                                 " FROM commonMealGuestRegistration " +
                                 " WHERE registrationId = @RegistrationId ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = await connection.QueryAsync<CommonMealGuestRegistrationDTO>(query, new {RegistrationId = registrationId});
                return Map(registrations.SingleOrDefault());
            }
        }

        public async Task<CommonMealGuestRegistration> Add(CommonMealGuestRegistration reg) {
            const string query =
                " INSERT INTO commonMealGuestRegistration (registrationId, Data) " +
                " VALUES (@RegistrationId, @Data) " +
                " RETURNING id ";

            using (var connection = _connectionFactory.New())
            {
                var id = await connection.QueryAsync<int>(query, new
                {
                    RegistrationId = reg.RegistrationId,
                    Data = MapData(reg)                    
                });

                reg.Id = id.Single();
                return reg;
            }
        }

        public async Task<CommonMealGuestRegistration> Update(CommonMealGuestRegistration reg) {
             const string query =
                " UPDATE commonMealGuestRegistration " +
                " SET Data = @data, registrationId = @regId " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new
                {
                    Id = reg.Id,
                    RegId = reg.RegistrationId,
                    Data = MapData(reg)                    
                });

                return reg;
            }
        }

        private string MapData(CommonMealGuestRegistration reg)
        {
            var sb = new StringBuilder();

            sb.Append($"ADULTS,CONVENTIONAL={reg.Adults.Conventional};");
            sb.Append($"ADULTS,VEGETARIAN={reg.Adults.Vegetarians};");
            sb.Append($"CHILDREN,CONVENTIONAL={reg.Children.Conventional};");
            sb.Append($"CHILDREN,VEGETARIAN={reg.Children.Vegetarians};");            
           
            return sb.ToString();
        }

        private CommonMealGuestRegistration Map(CommonMealGuestRegistrationDTO reg)
        {
            if (reg == null) return null;
            
            var lookup = reg.Data.AsKeyValuePairs(";").ToDictionary(x => x.Key, x => Convert.ToInt32(x.Value));

            return new CommonMealGuestRegistration {
                Id = reg.Id,
                RegistrationId = reg.RegistrationId,
                Adults = new PersonGroup {
                    Conventional = lookup["ADULTS,CONVENTIONAL"],
                    Vegetarians = lookup["ADULTS,VEGETARIAN"]
                },
                Children = new PersonGroup {
                    Conventional = lookup["CHILDREN,CONVENTIONAL"],
                    Vegetarians = lookup["CHILDREN,VEGETARIAN"]    
                }
            };
        }
    }    

    internal class CommonMealGuestRegistrationDTO {
        public int Id {get; set;}
        public int RegistrationId { get; set;}
        public string Data { get; set; }
    }    
}