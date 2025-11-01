using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using UIAuto.Utilities;

namespace UIAuto.Drivers
{
    public sealed class DriverManager
    {
        private static ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        private DriverManager()
        { }

        public static IWebDriver GetDriver()
        {
            if (_driver.Value == null)
            {
                throw new InvalidOperationException("Driver not initialized. Call InitializeDriver first.");
            }
            return _driver.Value;
        }

        public static void InitializeDriver(string browserType)
        {
            if (_driver.Value == null)
            {
                switch (browserType.ToLower())
                {
                    case "chrome":
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArgument("--start-maximized");
                        chromeOptions.AddArgument("--disable-notifications");
                        _driver.Value = new ChromeDriver(chromeOptions);
                        break;

                    case "firefox":
                        var firefoxOptions = new FirefoxOptions();
                        _driver.Value = new FirefoxDriver(firefoxOptions);
                        break;

                    case "edge":
                        var edgeOptions = new EdgeOptions();
                        _driver.Value = new EdgeDriver(edgeOptions);
                        break;

                    default:
                        throw new ArgumentException($"Browser type '{browserType}' is not supported.");
                }

                _driver.Value.Manage().Window.Maximize();
                _driver.Value = new AutoHighlightDriver()
                    .WrapDriverWithAutoHighlight(_driver.Value);
            }
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Value.Quit();
                _driver.Value.Dispose();
                _driver.Value = null;
            }
        }

        public static void DisposeAllDrivers()
        {
            if (_driver != null)
            {
                _driver.Dispose();
            }
        }
    }
}