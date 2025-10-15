using Microsoft.Extensions.Configuration;

namespace UIAuto.Utilities
{
    public class ConfigReader
    {
        private static IConfiguration _configuration;

        static ConfigReader()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetBaseUrl() => _configuration["TestSettings:BaseUrl"];

        public static string GetBrowser() => _configuration["TestSettings:Browser"];

        public static int GetImplicitWait() => int.Parse(_configuration["TestSettings:ImplicitWait"]);

        public static int GetExplicitWait() => int.Parse(_configuration["TestSettings:ExplicitWait"]);

        public static int GetPageLoadTimeout() => int.Parse(_configuration["TestSettings:PageLoadTimeout"]);

        public static string GetValidUsername() => _configuration["Credentials:ValidUsername"];

        public static string GetValidPassword() => _configuration["Credentials:ValidPassword"];

        public static string GetInvalidUsername() => _configuration["Credentials:InvalidUsername"];

        public static string GetInvalidPassword() => _configuration["Credentials:InvalidPassword"];

        public static string GetLogPath() => _configuration["Logging:LogPath"];

        public static string GetValue(string key) => _configuration[key];
    }
}