using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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

        public string GetCurrentUrl() => Driver.Url;

        public void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Logger.Info($"Navigated to URL: {url}");
        }

        protected void Click(By locator)
        {
            try
            {
                var element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                element.Click();
                Logger.Info($"Clicked element: {locator}");
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Error(ex, $"Timeout clicking element: {locator}");
                throw;
            }
            catch (NoSuchElementException ex)
            {
                Logger.Error(ex, $"Element not found: {locator}");
                throw;
            }
        }

        protected string GetText(By locator)
        {
            try
            {
                var element = Wait.Until(ExpectedConditions.ElementIsVisible(locator));
                string text = element.Text;
                Logger.Info($"Retrieved text '{text}' from element: {locator}");
                return text;
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Error(ex, $"Timeout getting text from element: {locator}");
                throw;
            }
            catch (NoSuchElementException ex)
            {
                Logger.Error(ex, $"Element not found: {locator}");
                throw;
            }
        }

        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                var element = Wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        protected void SendKeys(By locator, string text)
        {
            try
            {
                var element = Wait.Until(ExpectedConditions.ElementIsVisible(locator));
                element.Clear();
                element.SendKeys(text);
                Logger.Info($"Entered text '{text}' in element: {locator}");
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Error(ex, $"Timeout entering text in element: {locator}");
                throw;
            }
            catch (NoSuchElementException ex)
            {
                Logger.Error(ex, $"Element not found: {locator}");
                throw;
            }
        }
    }
}