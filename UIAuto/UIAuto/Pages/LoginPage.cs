using OpenQA.Selenium;
using UIAuto.Utilities;

namespace UIAuto.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By _usernameInput = By.Id("username");
        private readonly By _passwordInput = By.Id("password");
        private readonly By _submitButton = By.Id("submit");
        private readonly By _successMessage = By.CssSelector(".post-title");
        private readonly By _errorMessage = By.Id("error");
        private readonly By _logoutButton = By.LinkText("Log out");

        public void NavigateToLoginPage()
        {
            string url = ConfigReader.BaseUrl;
            NavigateToUrl(url); // Inherited from BasePage
            Logger.Info($"Navigated to Login Page at: {url}");
        }

        public LoginPage EnterUsername(string username)
        {
            SendKeys(_usernameInput, username); // Inherited from BasePage
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            SendKeys(_passwordInput, password); // Inherited from BasePage
            return this;
        }

        public void ClickSubmit() => Click(_submitButton); // Inherited from BasePage

        public void Login(string username, string password)
        {
            Logger.Info($"Attempting login with username: {username}");

            EnterUsername(username)
                .EnterPassword(password)
                .ClickSubmit();
        }

        public bool IsSuccessMessageDisplayed() => IsElementDisplayed(_successMessage);

        public string GetSuccessMessage() => GetText(_successMessage);

        public bool IsErrorMessageDisplayed() => IsElementDisplayed(_errorMessage);

        public string GetErrorMessage() => GetText(_errorMessage);

        public bool IsLogoutButtonDisplayed() => IsElementDisplayed(_logoutButton);

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