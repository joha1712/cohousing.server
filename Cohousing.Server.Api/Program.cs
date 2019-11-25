using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Cohousing.Server.Api.Startup;
using System.Globalization;

namespace Cohousing.Server.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("da-DK");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddCommandLine(args)
                .Build();           
                  
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(new[] { AppSettings.GetApiWebHostUrl(config)})                
                .UseStartup<Startup.Startup>();
        }
    }
}
