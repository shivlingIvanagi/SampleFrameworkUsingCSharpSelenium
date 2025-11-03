using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace UIAuto.Utilities
{
    public class AutoHighlightDriver
    {
        private EventFiringWebDriver _eventDriver;
        private ElementHighlighter _highlighter;

        public IWebDriver WrapDriverWithAutoHighlight(IWebDriver driver)
        {
            // Optimization: Only initialize the highlighter and event driver.
            // The base driver is captured by the EventFiringWebDriver instance.
            _eventDriver = new EventFiringWebDriver(driver);
            _highlighter = new ElementHighlighter(driver);

            // Subscribe to events
            _eventDriver.ElementClicking += OnElementClicking;
            _eventDriver.ElementClicked += OnElementClicked;
            _eventDriver.ElementValueChanging += OnElementValueChanging;

            Console.WriteLine("✅ Auto-Highlight Driver ACTIVATED");
            Console.WriteLine("All elements involved in actions will be highlighted automatically!\n");

            return _eventDriver;
        }

        public void Unsubscribe()
        {
            if (_eventDriver != null)
            {
                _eventDriver.ElementClicking -= OnElementClicking;
                _eventDriver.ElementClicked -= OnElementClicked;
                _eventDriver.ElementValueChanging -= OnElementValueChanging;
            }
        }

        private void OnElementClicking(object sender, WebElementEventArgs e)
        {
            // Optimization: Use the dedicated highlighter methods
            Console.WriteLine($"👆 Clicking: {GetElementInfo(e.Element)}");
            _highlighter.HighlightClick(e.Element);
        }

        private void OnElementClicked(object sender, WebElementEventArgs e)
        {
            Console.WriteLine($"✅ Clicked successfully: {GetElementInfo(e.Element)}\n");
        }

        private void OnElementValueChanging(object sender, WebElementValueEventArgs e)
        {
            Console.WriteLine($"⌨️ Typing into: {GetElementInfo(e.Element)}");
            _highlighter.HighlightType(e.Element);
        }

        private string GetElementInfo(IWebElement element)
        {
            try
            {
                // Optimization: Use interpolation and consolidate logic for readability and conciseness
                string tag = element.TagName;
                string id = element.GetAttribute("id");
                string text = element.Text?.Trim();

                if (!string.IsNullOrEmpty(id))
                    return $"<{tag} id='{id}'>";

                // Optimization: Check for innerText if .Text is empty for input elements
                string innerText = element.GetAttribute("innerText")?.Trim();
                if (!string.IsNullOrEmpty(innerText) && innerText.Length < 20)
                    return $"<{tag}> text: '{innerText}'";

                if (!string.IsNullOrEmpty(text) && text.Length < 20)
                    return $"<{tag}> text: '{text}'";

                return $"<{tag}>";
            }
            // Optimization: Catch specific exceptions and return "Unknown"
            catch (StaleElementReferenceException)
            {
                return "Stale/Detached element";
            }
            catch (Exception)
            {
                return "Unknown element";
            }
        }
    }
}