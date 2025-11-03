using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using UIAuto.Utilities;

namespace UIAuto.Drivers
{
    public static class DriverManager
    {
        // Optimization 2: Use ThreadLocal<IWebDriver> initialized with a default value of null, simplifying GetDriver.

        private static readonly ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        // Retrieves the WebDriver instance associated with the current thread.

        public static IWebDriver GetDriver()
        {
            return _driver.Value ??
                throw new InvalidOperationException("Driver not initialized. Call InitializeDriver first.");
        }

        // Initializes the browser driver for the current thread.

        public static void InitializeDriver(string browserType)
        {
            // Optimization: Use null check and short-circuit to prevent re-initialization.
            if (_driver.Value != null)
            {
                Logger.Warning("Driver is already initialized for the current thread. Skipping re-initialization.");
                return;
            }

            IWebDriver driver;

            driver = browserType.ToLower() switch
            {
                "chrome" => CreateChromeDriver(),
                "firefox" => CreateFirefoxDriver(),
                "edge" => CreateEdgeDriver(),
                _ => throw new ArgumentException($"Browser type '{browserType}' is not supported."),
            };

            driver.Manage().Window.Maximize();

            // Optimization 5: Wrap the driver using the AutoHighlight utility.
            driver = new AutoHighlightDriver().WrapDriverWithAutoHighlight(driver);

            // Set the final wrapped driver to the ThreadLocal storage.
            _driver.Value = driver;
        }

        private static ChromeDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            // Optimization: Maximize is often done here for cross-browser consistency, but we keep it minimal.
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            // Optimization: Consider adding --headless, --incognito, or --disable-gpu options here if common.
            return new ChromeDriver(options);
        }

        private static FirefoxDriver CreateFirefoxDriver()
        {
            // Optimization: Add maximization if it wasn't done globally.
            // var options = new FirefoxOptions();
            // options.AddArgument("--width=1920"); // Example if Maximize() fails
            // return new FirefoxDriver(options);
            return new FirefoxDriver();
        }

        private static EdgeDriver CreateEdgeDriver()
        {
            // var options = new EdgeOptions();
            // return new EdgeDriver(options);
            return new EdgeDriver();
        }

        // --- Cleanup Methods --

        /// Quits and Disposes the driver for the current thread.

        public static void QuitDriver()
        {
            IWebDriver currentDriver = _driver.Value;

            if (currentDriver == null)
                return;

            try
            {
                currentDriver.Quit();
                currentDriver.Dispose();
            }
            catch (Exception ex)
            {
                // Only log the exception; do not re-throw, as teardown should not fail the test suite.
                Logger.Error(ex, "Error occurred during driver Quit or Dispose.");
            }
            finally
            {
                //Ensure ThreadLocal value is cleared regardless of Quit/Dispose success.
                _driver.Value = null;
            }
        }

        /// Disposes the underlying ThreadLocal resources when the entire test suite is complete.

        public static void DisposeAllDrivers()
        {
            if (_driver != null)
            {
                try
                {
                    // This disposes the ThreadLocal wrapper itself, which should ideally be done once at the very end.
                    _driver.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error occurred during ThreadLocal driver disposal.");
                }
            }
        }
    }
}