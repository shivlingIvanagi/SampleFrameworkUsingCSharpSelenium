using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UIAuto.Drivers;
using UIAuto.Utilities;

namespace UIAuto.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
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
            string browser = ConfigReader.Browser;

            try
            {
                Logger.Info($"Initializing {browser} browser for test: {TestContext.CurrentContext.Test.Name}");

                // 1. Initialize Driver
                DriverManager.InitializeDriver(browser);
                var driver = DriverManager.GetDriver();

                TimeSpan implicitWait = TimeSpan.FromSeconds(ConfigReader.ImplicitWait);
                TimeSpan pageLoadTimeout = TimeSpan.FromSeconds(ConfigReader.PageLoadTimeout);

                driver.Manage().Timeouts().ImplicitWait = implicitWait;
                driver.Manage().Timeouts().PageLoad = pageLoadTimeout;

                // driver.Manage().Window.Maximize();

                Logger.Info($"Browser initialized successfully with Implicit Wait: {implicitWait.TotalSeconds}s and Page Load Timeout: {pageLoadTimeout.TotalSeconds}s");
            }
            catch (Exception ex)
            {
                DriverManager.QuitDriver();
                Logger.Error(ex, $"Failed to initialize {browser} browser.");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            TestStatus testStatus = TestContext.CurrentContext.Result.Outcome.Status;

            try
            {
                if (testStatus == TestStatus.Failed)
                {
                    string testName = TestContext.CurrentContext.Test.Name;
                    Logger.Error($"Test '{testName}' FAILED! Capturing screenshot...");
                    ScreenshotHelper.CaptureScreenshot(testName);
                }

                Logger.Info($"Closing browser after test status: {testStatus}");
                DriverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during browser cleanup in TearDown. Driver may be orphaned.");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DriverManager.DisposeAllDrivers();
            Logger.Info("=== Test Suite Execution Completed ===");
        }
    }
}