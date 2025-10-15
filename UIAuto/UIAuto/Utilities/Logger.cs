using Serilog;

namespace UIAuto.Utilities
{
    public class Logger
    {
        private static ILogger _logger;

        static Logger()
        {
            string logPath = ConfigReader.GetLogPath();
            string logDirectory = Path.GetDirectoryName(logPath);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        public static void Info(string message) => _logger.Information(message);

        public static void Debug(string message) => _logger.Debug(message);

        public static void Warning(string message) => _logger.Warning(message);

        public static void Error(string message) => _logger.Error(message);

        public static void Error(Exception ex, string message) => _logger.Error(ex, message);
    }
}