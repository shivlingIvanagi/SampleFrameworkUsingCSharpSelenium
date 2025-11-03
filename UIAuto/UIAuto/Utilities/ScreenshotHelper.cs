using OpenQA.Selenium;
using UIAuto.Drivers;

namespace UIAuto.Utilities
{
    public static class ScreenshotHelper
    {
        private const string ScreenshotPath = "Screenshots";

        public static string CaptureScreenshot(string testName)
        {
            try
            {
                var driver = DriverManager.GetDriver();
                if (driver is not ITakesScreenshot screenshotDriver)
                {
                    Logger.Error("Driver does not support screenshot capability");
                    return null;
                }

                // Ensure directory exists (thread-safe)
                Directory.CreateDirectory(ScreenshotPath);

                var screenshot = screenshotDriver.GetScreenshot();

                // Sanitize testName to remove invalid file name characters
                string safeTestName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
                string fileName = $"{safeTestName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string filePath = Path.Combine(ScreenshotPath, fileName);

                screenshot.SaveAsFile(filePath);
                Logger.Info($"Screenshot captured: {filePath}");

                return filePath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to capture screenshot");
                return null;
            }
        }
    }
}