using OpenQA.Selenium;

namespace UIAuto.Utilities
{
    public class ElementHighlighter
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;
        private const int ScrollWaitMs = 200;

        public ElementHighlighter(IWebDriver driver)
        {
            _driver = driver;

            _jsExecutor = driver as IJavaScriptExecutor ??
                throw new ArgumentException("Driver must implement IJavaScriptExecutor for highlighting.");
        }

        public void HighlightElement(IWebElement element, string color = "red",
            int borderWidth = 3, int durationMs = 1000, bool scrollToView = true)
        {
            if (scrollToView)
            {
                _jsExecutor.ExecuteScript(
                    "arguments[0].scrollIntoView({behavior: 'smooth', block: 'center', inline: 'center'});",
                    element);
                Thread.Sleep(ScrollWaitMs);
            }

            // Non-blocking JavaScript for highlighting
            string highlightStyle = $"border: {borderWidth}px solid {color}; box-shadow: 0 0 10px {color};";

            string script =
                "var original_style = arguments[0].getAttribute('style');" +
                "arguments[0].setAttribute('original-style', original_style);" +
                "arguments[0].setAttribute('style', original_style + arguments[1]);" +
                "setTimeout(function() {" +
                    "var restored_style = arguments[0].getAttribute('original-style');" +
                    "arguments[0].setAttribute('style', restored_style);" +
                    "arguments[0].removeAttribute('original-style');" +
                "}, arguments[2]);";

            _jsExecutor.ExecuteScript(
                script,
                element, highlightStyle, durationMs);
        }

        public void HighlightElement(By locator, string color = "red",
            int borderWidth = 3, int durationMs = 1000)
        {
            IWebElement element = _driver.FindElement(locator);
            HighlightElement(element, color, borderWidth, durationMs);
        }

        // ... (HighlightClick, HighlightType, etc. methods remain the same)
        public void HighlightClick(IWebElement element) => HighlightElement(element, "blue", 4, 800);

        public void HighlightType(IWebElement element) => HighlightElement(element, "green", 4, 800);

        public void HighlightError(IWebElement element) => HighlightElement(element, "red", 5, 1500);

        public void HighlightSuccess(IWebElement element) => HighlightElement(element, "lime", 4, 800);
    }
}