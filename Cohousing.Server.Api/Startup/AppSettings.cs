using System;
using Cohousing.Server.SqlRepository;
using Microsoft.Extensions.Configuration;

namespace Cohousing.Server.Api.Startup
{
    public class AppSettings : ISqlRepositorySettings, ICommonMealSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetSection("AppSettings:DbConnectionString").Value;

        public TimeSpan MealTime => TimeSpan.Parse(_configuration.GetSection("AppSettings:CommonMealTime").Value);
    }

    public interface ICommonMealSettings
    {
        TimeSpan MealTime { get; }
    }
}