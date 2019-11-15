using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;
using HerokuNpgSql;
using Microsoft.Extensions.Configuration;

namespace Cohousing.Server.Api.Startup
{  
    public class AppSettings : ICommonMealSettings
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<IImmutableList<KeyValuePair<DayOfWeek,TimeSpan>>> _cachedDefaultDinnerDates;
        private readonly Lazy<int> _cachedDefaultDaysToLoad;
        private readonly Lazy<int> _cachedNumberOfChefs;
        private readonly Lazy<string> _cachedConnectionString;

        public AppSettings(IConfiguration configuration, IConfigRepository configRepository)
        {
            _configuration = configuration;
            
            _cachedConnectionString = new Lazy<string>(() => GetConnectionString(configuration));

            _cachedNumberOfChefs = new Lazy<int>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealNumberOfChefs")?.Value;
                return valueAsString != null ? Convert.ToInt32(valueAsString) : 1;
            });

            _cachedDefaultDaysToLoad = new Lazy<int>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealDefaultDaysToLoad")?.Value;
                return valueAsString != null ? Convert.ToInt32(valueAsString) : 7;
            });

            _cachedDefaultDinnerDates = new Lazy<IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>>>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealDefaultDinnerDates").Value;
                var valueAsKeyValuePairs = valueAsString.AsKeyValuePairs();
                return valueAsKeyValuePairs
                    .Select(x =>
                    {
                        var key = Enum.Parse<DayOfWeek>(x.Key, true);
                        var value = TimeSpan.Parse(x.Value);
                        return new KeyValuePair<DayOfWeek, TimeSpan>(key, value);
                    })
                    .ToImmutableList();
            });
        }

        // ReSharper disable once UnusedMember.Global
        public string ConnectionString => _cachedConnectionString.Value;

        // ReSharper disable once UnusedMember.Global
        public string ApiUrl => GetApiWebHostUrl(_configuration);

        public int NumberOfChefs => _cachedNumberOfChefs.Value;
        public int DefaultDaysToLoad => _cachedDefaultDaysToLoad.Value;

        public IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> DefaultDinnerDates => _cachedDefaultDinnerDates.Value;

        public static string GetConnectionString(IConfiguration configuration)
        {
            // Use heroku DATABASE_URL environment variable if present
            var herokuConnection = ReadHerokuSetting("DATABASE_URL");
            if (!string.IsNullOrEmpty(herokuConnection))
            {
                var connection = new HerokuNpgSqlConnectionStringBuilder(herokuConnection);
                return connection.ConnectionString;
            }
                
            // Use fallback from app config
            return configuration.GetSection("AppSettings:DbConnectionString").Value;
        }

        public static string GetApiWebHostUrl(IConfiguration configuration)
        {
            return ReadHerokuSetting("HOST_URL") ?? configuration.GetSection("AppSettings:ApiWebHostUrl").Value;
        }    
        
        static string ReadHerokuSetting(string herokuKey)
        {
            // Read Heroku setting from environment
            return Environment.GetEnvironmentVariable(herokuKey.ToUpperInvariant());
        }
    }
}