using NUnit.Framework;
using UIAuto.Drivers;
using UIAuto.Utilities;

namespace UIAuto.Tests
{
    [TestFixture]
    public class BaseTest
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Logger.Info("=== Test Suite Execution Started ===");
        }

        [SetUp]
        public void Setup()
        {
            try
            {
                string browser = ConfigReader.GetBrowser();
                Logger.Info($"Initializing {browser} browser");

                DriverManager.InitializeDriver(browser);

                var driver = DriverManager.GetDriver();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigReader.GetImplicitWait());
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigReader.GetPageLoadTimeout());

                Logger.Info("Browser initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to initialize browser");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string testName = TestContext.CurrentContext.Test.Name;
                    Logger.Warning($"Test '{testName}' failed. Capturing screenshot...");
                    ScreenshotHelper.CaptureScreenshot(testName);
                }

                Logger.Info("Closing browser");
                DriverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during teardown");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Logger.Info("=== Test Suite Execution Completed ===");
        }
    }
}