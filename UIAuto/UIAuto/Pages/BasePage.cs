using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using UIAuto.Drivers;
using UIAuto.Utilities;

namespace UIAuto.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage()
        {
            Driver = DriverManager.GetDriver();
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(ConfigReader.GetExplicitWait()));
        }

        public string GetCurrentUrl()
        {
            return Driver.Url;
        }

        public void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Logger.Info($"Navigated to URL: {url}");
        }

        protected void Click(By locator)
        {
            try
            {
                Wait.Until(d => d.FindElement(locator)).Click();
                Logger.Info($"Clicked element: {locator}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to click element: {locator}");
                throw;
            }
        }

        protected string GetText(By locator)
        {
            try
            {
                string text = Wait.Until(d => d.FindElement(locator)).Text;
                Logger.Info($"Retrieved text '{text}' from element: {locator}");
                return text;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to get text from element: {locator}");
                throw;
            }
        }

        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                return Wait.Until(d => d.FindElement(locator)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        protected void SendKeys(By locator, string text)
        {
            try
            {
                var element = Wait.Until(d => d.FindElement(locator));
                element.Clear();
                element.SendKeys(text);
                Logger.Info($"Entered text '{text}' in element: {locator}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to enter text in element: {locator}");
                throw;
            }
        }
    }
}