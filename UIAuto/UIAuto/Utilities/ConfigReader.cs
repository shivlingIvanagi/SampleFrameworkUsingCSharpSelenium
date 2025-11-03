using Microsoft.Extensions.Configuration;

namespace UIAuto.Utilities
{
    public static class ConfigReader // Optimization 1: Changed to a static class
    {
        private static readonly IConfiguration _configuration; // Optimization 2: Added readonly

        // Optimization: Use strongly-typed properties for cleaner access instead of methods

        public static string BaseUrl => GetString("TestSettings:BaseUrl");
        public static string Browser => GetString("TestSettings:Browser");

        // Optimization 3: Use private helper methods for type conversion and error handling
        public static int ImplicitWait => GetInt("TestSettings:ImplicitWait");

        public static int ExplicitWait => GetInt("TestSettings:ExplicitWait");
        public static int PageLoadTimeout => GetInt("TestSettings:PageLoadTimeout");

        public static string ValidUsername => GetString("Credentials:ValidUsername");
        public static string ValidPassword => GetString("Credentials:ValidPassword");
        public static string InvalidUsername => GetString("Credentials:InvalidUsername");
        public static string InvalidPassword => GetString("Credentials:InvalidPassword");

        public static string LogPath => GetString("Logging:LogPath");

        // Static constructor runs once when the class is first accessed
        static ConfigReader()
        {
            // Optimization 4: Use 'AppContext.BaseDirectory' for consistent path resolution, especially in tests.
            // Using a full path makes the test project setup more reliable across environments.
            var basePath = AppContext.BaseDirectory;

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Optimization 5: Add a check to fail fast if the configuration file is missing/empty
            if (_configuration == null || !_configuration.GetChildren().GetEnumerator().MoveNext())
            {
                throw new InvalidOperationException($"Configuration file 'Config/appsettings.json' not found or is empty in base directory: {basePath}");
            }
        }

        /// Reads a string value from configuration and ensures it's not null or empty.

        public static string GetString(string key)
        {
            string value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(key, $"Configuration key '{key}' not found or value is empty.");
            }
            return value;
        }

        /// Reads an integer value from configuration, handling potential parsing errors.

        public static int GetInt(string key)
        {
            string value = GetString(key); // Use GetString to validate existence first

            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"Configuration key '{key}' value ('{value}') is not a valid integer.");
            }
        }

        // This is the optimized version of the original GetValue, kept for backward compatibility/flexibility
        public static string GetValue(string key) => _configuration[key];
    }
}