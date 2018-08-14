using System;
using Cohousing.Server.SqlRepository;
using Microsoft.Extensions.Configuration;

namespace Cohousing.Server.Api.Startup
{
    public class AppConfig : ISqlRepositorySettings
    {
        private readonly IConfiguration _configuration;

        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetSection("AppSettings:DbConnectionString").Value;
    }
}