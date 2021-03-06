﻿using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;
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
            const string query = " SELECT id, attending, personId, guests, commonmealid, takeaway, date " +
                                 " FROM commonMealRegistration " +
                                 " WHERE id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = (await connection.QueryAsync(query, new {Id = id}))
                    .Select(row => new CommonMealRegistration {
                        Id = row.id,
                        Attending = row.attending,
                        PersonId = row.personid,
                        CommonMealId = row.commonmealid,
                        Timestamp = row.date,
                        IsTakeAway = row.takeaway,
                        Guests = MapGuests(row.guests, row.id)
                    }); 
                var result = registrations.SingleOrDefault();                
                return result;
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> GetByCommonMealId(int commonMealId)
        {
            const string query = " SELECT id, attending, personid, guests, commonmealid, takeaway, date " +
                                 " FROM commonMealRegistration " +
                                 " WHERE commonMealId = @CommonMealId " + 
                                 " ORDER BY personId asc ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = (await connection.QueryAsync(query, new {CommonMealId = commonMealId}))                
                    .Select(row => new CommonMealRegistration {
                        Id = row.id,
                        Attending = row.attending,
                        PersonId = row.personid,
                        CommonMealId = row.commonmealid,
                        Timestamp = row.date,
                        IsTakeAway = row.takeaway,
                        Guests = MapGuests(row.guests, row.id)
                    });                
                
                return registrations.ToImmutableList();
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> GetAll()
        {
            const string query = " SELECT id, attending, personId, guests, commonmealid, takeaway, date " +
                                 " FROM CommonMealRegistration " +
                                 " ORDER BY personId asc ";

            using (var connection = _connectionFactory.New())
            {
                var registrations = (await connection.QueryAsync(query))
                    .Select(row => new CommonMealRegistration {
                        Id = row.id,
                        Attending = row.attending,
                        PersonId = row.personid,
                        CommonMealId = row.commonmealid,
                        Timestamp = row.date,
                        IsTakeAway = row.takeaway,
                        Guests = MapGuests(row.guests, row.id)
                    });
               
                return registrations.ToImmutableList();
            }
        }

        public async Task<CommonMealRegistration> Add(CommonMealRegistration registration)
        {
            const string query =
                " INSERT INTO commonMealRegistration (personId, commonMealId, attending, guests, date, takeaway) " +
                " VALUES (@PersonId, @CommonMealId, @Attending, @Guests, @Date, @TakeAway) " +
                " RETURNING id ";

            using (var connection = _connectionFactory.New())
            {
                var id = await connection.QueryAsync<int>(query, new
                {
                    PersonId = registration.PersonId,
                    Attending = registration.Attending,
                    CommonMealId = registration.CommonMealId,
                    Date = registration.Timestamp,
                    Guests = MapToGuestString(registration.Guests),
                    TakeAway = registration.IsTakeAway
                });

                registration.Id = id.Single();

                return registration;
            }
        }

        public async Task<IImmutableList<CommonMealRegistration>> AddMany(IImmutableList<CommonMealRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                await Add(registration);
            }

            return registrations;
        }

        public async Task<CommonMealRegistration> Update(CommonMealRegistration registration)
        {
            const string query =
                " UPDATE CommonMealRegistration " +
                " SET personId = @PersonId, attending = @Attending, guests = @Guests, date = @Date, takeaway = @TakeAway " +
                " WHERE Id = @Id ";

            using (var connection = _connectionFactory.New())
            {
                await connection.QueryAsync(query, new
                {
                    Id = registration.Id,
                    PersonId = registration.PersonId,
                    Attending = registration.Attending,
                    Date =  registration.Timestamp,
                    Guests = MapToGuestString(registration.Guests),
                    TakeAway = registration.IsTakeAway
                });

                return registration;
            }
        }
        private string MapToGuestString(CommonMealGuestRegistration reg)
        {
            var sb = new StringBuilder();

            sb.Append($"ADULTS,CONVENTIONAL={reg.Adults.Conventional};");
            sb.Append($"ADULTS,VEGETARIAN={reg.Adults.Vegetarians};");
            sb.Append($"CHILDREN,CONVENTIONAL={reg.Children.Conventional};");
            sb.Append($"CHILDREN,VEGETARIAN={reg.Children.Vegetarians};");            
           
            return sb.ToString();
        }

        private CommonMealGuestRegistration MapGuests(string guestsBlob, int regId)
        {
            if (guestsBlob == null) {
                return new CommonMealGuestRegistration() {
                    RegistrationId = regId,
                    Adults = new PersonGroup(),
                    Children = new PersonGroup(),
                };
            }
            
            var lookup = guestsBlob.AsKeyValuePairs(";").ToDictionary(x => x.Key, x => int.TryParse(x.Value, out var f) ? f : default(int?));

            return new CommonMealGuestRegistration {
                RegistrationId = regId,
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
}