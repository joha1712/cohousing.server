using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.SqlRepository;
using Cohousing.Server.Util;
using Microsoft.Extensions.Configuration;

namespace Cohousing.Server.Api.Startup
{
    public class AppSettings : ISqlRepositorySettings, ICommonMealSettings
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<IImmutableList<KeyValuePair<DayOfWeek,TimeSpan>>> _cachedDefaultCommonMealDates;
        private readonly Lazy<int> _cachedDefaultDaysShown;
        private readonly Lazy<int> _cachedNumberOfChefs;

        public AppSettings(IConfiguration configuration, IConfigRepository configRepository)
        {
            _configuration = configuration;

            _cachedNumberOfChefs = new Lazy<int>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealNumberOfChefs")?.Value;
                return valueAsString != null ? Convert.ToInt32(valueAsString) : 1;
            });

            _cachedDefaultDaysShown = new Lazy<int>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealDefaultDaysShown")?.Value;
                return valueAsString != null ? Convert.ToInt32(valueAsString) : 5;
            });

            _cachedDefaultCommonMealDates = new Lazy<IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>>>(() =>
            {
                var valueAsString = configRepository.GetByKey("CommonMealDefaultDates").Value;
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

        public string ConnectionString => GetConnectionString(_configuration);

        public int NumberOfChefs => _cachedNumberOfChefs.Value;
        public int DefaultDaysShown => _cachedDefaultDaysShown.Value;

        public IImmutableList<KeyValuePair<DayOfWeek, TimeSpan>> DefaultCommonMealDates => _cachedDefaultCommonMealDates.Value;

        public static string GetConnectionString(IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings:DbConnectionString").Value;
        }
    }
}