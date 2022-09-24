using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Screen.Core
{
    public static class ConfigurationHelper
    {

        public static IConfiguration _config { get; private set; }

        public static string GetByName(string configKeyName)
        {
            if (_config == null)
            {
                _config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            }

            IConfigurationSection section = _config.GetSection(configKeyName);

            return section.Value;
        }
    }
}
