using OpenQA.Selenium;

namespace UIAuto.Utilities
{
    public class ElementHighlighter
    {
        private IWebDriver _driver;
        private IJavaScriptExecutor _jsExecutor;

        public ElementHighlighter(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// Highlights an element with a colored border.
        /// Default: Red border, 3px thick, 1 second duration
        /// </summary>
        public void HighlightElement(IWebElement element, string color = "red",
            int borderWidth = 3, int durationMs = 1000, bool scrollToView = true)
        {
            if (scrollToView)
            {
                _jsExecutor.ExecuteScript(
                    "arguments[0].scrollIntoView({behavior: 'smooth', block: 'center', inline: 'center'});",
                    element);

                // Wait for scroll animation to complete
                Thread.Sleep(300);
            }

            // Store original style
            string originalStyle = element.GetAttribute("style");

            // Apply highlight style
            string highlightStyle = $"border: {borderWidth}px solid {color}; " +
                                   $"box-shadow: 0 0 10px {color};";

            _jsExecutor.ExecuteScript(
                "arguments[0].setAttribute('style', arguments[1]);",
                element, originalStyle + highlightStyle);

            // Wait for specified duration
            Thread.Sleep(durationMs);

            // Restore original style
            _jsExecutor.ExecuteScript(
                "arguments[0].setAttribute('style', arguments[1]);",
                element, originalStyle);
        }

        /// <summary>
        /// Highlights element by locator
        /// </summary>
        public void HighlightElement(By locator, string color = "red",
            int borderWidth = 3, int durationMs = 1000)
        {
            IWebElement element = _driver.FindElement(locator);
            HighlightElement(element, color, borderWidth, durationMs);
        }

        /// <summary>
        /// Highlights element with custom colors for different actions
        /// </summary>
        public void HighlightClick(IWebElement element)
        {
            HighlightElement(element, "blue", 4, 800);
        }

        public void HighlightType(IWebElement element)
        {
            HighlightElement(element, "green", 4, 800);
        }

        public void HighlightError(IWebElement element)
        {
            HighlightElement(element, "red", 5, 1500);
        }

        public void HighlightSuccess(IWebElement element)
        {
            HighlightElement(element, "lime", 4, 800);
        }
    }
}