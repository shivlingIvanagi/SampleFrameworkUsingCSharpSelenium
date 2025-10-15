using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace UIAuto.Utilities
{
    public class AutoHighlightDriver
    {
        private EventFiringWebDriver _eventDriver;
        private IWebDriver _baseDriver;
        private ElementHighlighter _highlighter;

        public IWebDriver WrapDriverWithAutoHighlight(IWebDriver driver)
        {
            _baseDriver = driver;
            _eventDriver = new EventFiringWebDriver(_baseDriver);
            _highlighter = new ElementHighlighter(_baseDriver);

            // Subscribe to events
            _eventDriver.ElementClicking += OnElementClicking;
            _eventDriver.ElementClicked += OnElementClicked;
            _eventDriver.ElementValueChanging += OnElementValueChanging;
            _eventDriver.FindElementCompleted += OnFindElementCompleted;

            Console.WriteLine("✅ Auto-Highlight Driver ACTIVATED");
            Console.WriteLine("All elements will be highlighted automatically!\n");

            return _eventDriver;
        }

        private void OnElementClicking(object sender, WebElementEventArgs e)
        {
            Console.WriteLine($"👆 Clicking: {GetElementInfo(e.Element)}");
            _highlighter.HighlightElement(e.Element, "blue", 4, 500);
        }

        private void OnElementClicked(object sender, WebElementEventArgs e)
        {
            Console.WriteLine($"✅ Clicked successfully\n");
        }

        private void OnElementValueChanging(object sender, WebElementValueEventArgs e)
        {
            Console.WriteLine($"⌨️  Typing into: {GetElementInfo(e.Element)}");
            _highlighter.HighlightElement(e.Element, "green", 4, 500);
        }

        private void OnFindElementCompleted(object sender, FindElementEventArgs e)
        {
            // Briefly highlight found elements
            try
            {
                var element = _baseDriver.FindElement(e.FindMethod);
                _highlighter.HighlightElement(element, "yellow", 2, 300);
            }
            catch { }
        }

        private string GetElementInfo(IWebElement element)
        {
            try
            {
                string tag = element.TagName;
                string id = element.GetAttribute("id");
                string text = element.Text;

                if (!string.IsNullOrEmpty(id))
                    return $"<{tag} id='{id}'>";
                else if (!string.IsNullOrEmpty(text) && text.Length < 20)
                    return $"<{tag}> with text '{text}'";
                else
                    return $"<{tag}>";
            }
            catch
            {
                return "Unknown element";
            }
        }
    }
}