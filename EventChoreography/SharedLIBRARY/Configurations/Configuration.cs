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
                    .AddJsonFile("Json File Path")
                    .Build();
        }

        public static DbSettings GetDbSettings()
        {
            GetConfiguration();
            var url = _configuration["configuration"];
            return new() { ConnectionString = url };
        }

        public static RabbitMQSettings GetRabbitMQSettings()
        {
            GetConfiguration();
            var host = _configuration["configuration"];
            var username = _configuration["configuration"];
            var password = _configuration["configuration"];
            return new() { RabbitMqHost = host, RabbitMqPassword = password, RabbitMqUserName = username };
        }
    }
}
