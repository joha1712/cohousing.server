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

        public TimeSpan DefaultMealTime => TimeSpan.Parse(_configuration.GetSection("AppSettings:CommonMealDefaultTime").Value);
        public int DefaultNumberOfChefs => Convert.ToInt32(_configuration.GetSection("AppSettings:CommonMealDefaultNumberOfChefs").Value);
        public int DefaultDaysShown => Convert.ToInt32(_configuration.GetSection("AppSettings:CommonMealDefaultDaysShown").Value);
        
    }
}