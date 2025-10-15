using OpenQA.Selenium;
using UIAuto.Drivers;

namespace UIAuto.Utilities
{
    public class ScreenshotHelper
    {
        private static readonly string ScreenshotPath = "Screenshots";

        public static string CaptureScreenshot(string testName)
        {
            try
            {
                if (!Directory.Exists(ScreenshotPath))
                {
                    Directory.CreateDirectory(ScreenshotPath);
                }

                ITakesScreenshot screenshotDriver = (ITakesScreenshot)DriverManager.GetDriver();
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                string fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
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