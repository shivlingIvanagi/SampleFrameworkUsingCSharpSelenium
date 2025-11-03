using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using UIAuto.Drivers;
using UIAuto.Utilities;

namespace UIAuto.Pages
{
    public class BasePage
    {
        protected static IWebDriver Driver => DriverManager.GetDriver();
        protected static WebDriverWait Wait => new WebDriverWait(Driver, TimeSpan.FromSeconds(ConfigReader.ExplicitWait));

        private IWebElement WaitForElement(By locator, Func<IWebDriver, IWebElement> condition)
        {
            try
            {
                // This combines waiting and finding the element
                return Wait.Until(condition);
            }
            catch (WebDriverTimeoutException ex)
            {
                Logger.Error(ex, $"Timeout while waiting for element: {locator}");
                throw; // Re-throw the exception to allow calling methods to handle or fail the test
            }
            catch (NoSuchElementException ex)
            {
                // Catching NoSuchElementException here is less common for ExpectedConditions but good for robustness
                Logger.Error(ex, $"Element not found: {locator}");
                throw;
            }
        }

        public string GetCurrentUrl() => Driver.Url;

        public void NavigateToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Logger.Info($"Navigated to URL: {url}");
        }

        protected void Click(By locator)
        {
            var element = WaitForElement(locator, ExpectedConditions.ElementToBeClickable(locator));
            element.Click();
            Logger.Info($"Clicked element: {locator}");
        }

        protected string GetText(By locator)
        {
            var element = WaitForElement(locator, ExpectedConditions.ElementIsVisible(locator));
            string text = element.Text;
            Logger.Info($"Retrieved text '{text}' from element: {locator}");
            return text;
        }

        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                var element = WaitForElement(locator, ExpectedConditions.ElementIsVisible(locator));
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
            var element = WaitForElement(locator, ExpectedConditions.ElementIsVisible(locator));
            element.Clear();
            element.SendKeys(text);
            Logger.Info($"Entered text '{text}' in element: {locator}");
        }
    }
}