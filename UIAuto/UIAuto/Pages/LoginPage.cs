using OpenQA.Selenium;
using UIAuto.Utilities;

namespace UIAuto.Pages
{
    public class LoginPage : BasePage
    {
        // Locators
        private readonly By _usernameInput = By.Id("username");

        private readonly By _passwordInput = By.Id("password");
        private readonly By _submitButton = By.Id("submit");
        private readonly By _successMessage = By.CssSelector(".post-title");
        private readonly By _errorMessage = By.Id("error");
        private readonly By _logoutButton = By.LinkText("Log out");

        public LoginPage() : base()
        {
        }

        public void NavigateToLoginPage()
        {
            string url = ConfigReader.GetBaseUrl();
            NavigateToUrl(url);
            Logger.Info("Navigated to Login Page");
        }

        public void EnterUsername(string username)
        {
            SendKeys(_usernameInput, username);
        }

        public void EnterPassword(string password)
        {
            SendKeys(_passwordInput, password);
        }

        public void ClickSubmit()
        {
            Click(_submitButton);
        }

        public void Login(string username, string password)
        {
            Logger.Info($"Attempting login with username: {username}");
            EnterUsername(username);
            EnterPassword(password);
            ClickSubmit();
        }

        public bool IsSuccessMessageDisplayed()
        {
            return IsElementDisplayed(_successMessage);
        }

        public string GetSuccessMessage()
        {
            return GetText(_successMessage);
        }

        public bool IsErrorMessageDisplayed()
        {
            return IsElementDisplayed(_errorMessage);
        }

        public string GetErrorMessage()
        {
            return GetText(_errorMessage);
        }

        public bool IsLogoutButtonDisplayed()
        {
            return IsElementDisplayed(_logoutButton);
        }

        public void ClickLogout()
        {
            if (IsLogoutButtonDisplayed())
            {
                Click(_logoutButton);
                Logger.Info("Clicked logout button");
            }
        }
    }
}