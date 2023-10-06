using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SharedLIBRARY.Models;

namespace SharedLIBRARY.Configurations
{
    public static class Configuration
    {
        private static IConfiguration _configuration;

        private static void GetConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("C:\\Users\\Casper\\Desktop\\GitHub Projects\\SagaPattern\\Orchestration\\SharedLIBRARY\\appsettings.json")
                    .Build();
        }

        public static DbSettings GetDbSettings()
        {
            GetConfiguration();
            var url = _configuration["DbSettings:DbContext"];
            return new() { ConnectionString = url };
        }

        public static RabbitMQSettings GetRabbitMQSettings()
        {
            GetConfiguration();
            var host = _configuration["RabbitMQSetings:RabbitMqHost"];
            var username = _configuration["RabbitMQSetings:RabbitMqUsername"];
            var password = _configuration["RabbitMQSetings:RabbitMqPassword"];
            return new() { RabbitMqHost = host, RabbitMqPassword = password, RabbitMqUserName = username };
        }
    }
}
