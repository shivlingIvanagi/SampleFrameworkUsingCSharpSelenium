using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using UIAuto.Utilities;

namespace UIAuto.Drivers
{
    public sealed class DriverManager
    {
        private static IWebDriver _driver;

        private DriverManager()
        { }

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                throw new InvalidOperationException("Driver not initialized. Call InitializeDriver first.");
            }
            return _driver;
        }

        public static void InitializeDriver(string browserType)
        {
            if (_driver == null)
            {
                switch (browserType.ToLower())
                {
                    case "chrome":
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArgument("--start-maximized");
                        chromeOptions.AddArgument("--disable-notifications");
                        _driver = new ChromeDriver(chromeOptions);
                        break;

                    case "firefox":
                        var firefoxOptions = new FirefoxOptions();
                        _driver = new FirefoxDriver(firefoxOptions);
                        break;

                    case "edge":
                        var edgeOptions = new EdgeOptions();
                        _driver = new EdgeDriver(edgeOptions);
                        break;

                    default:
                        throw new ArgumentException($"Browser type '{browserType}' is not supported.");
                }

                _driver.Manage().Window.Maximize();
                _driver = new AutoHighlightDriver()
                    .WrapDriverWithAutoHighlight(_driver);
            }
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}