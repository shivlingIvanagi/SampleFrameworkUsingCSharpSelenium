using NUnit.Framework;
using UIAuto.Pages;
using UIAuto.Utilities;

namespace UIAuto.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage;

        [SetUp]
        public void LoginTestSetup()
        {
            _loginPage = new LoginPage();
        }

        [Test]
        [Category("Smoke")]
        [Category("Login")]
        [Description("Verify that user can login with valid credentials")]
        public void Test_LoginWithValidCredentials_Success()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Valid Credentials");
            string username = ConfigReader.GetValidUsername();
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsSuccessMessageDisplayed(), Is.True,
                    "Success message should be displayed after successful login");

                Assert.That(_loginPage.GetSuccessMessage(), Does.Contain("Logged In Successfully"),
                    "Success message text should contain 'Logged In Successfully'");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.True,
                    "Logout button should be displayed after successful login");

                Assert.That(_loginPage.GetCurrentUrl(), Does.Contain("logged-in-successfully"),
                    "URL should contain 'logged-in-successfully' after login");
            });

            Logger.Info("Test Passed: Login with Valid Credentials");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with invalid username")]
        public void Test_LoginWithInvalidUsername_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Invalid Username");
            string username = ConfigReader.GetInvalidUsername();
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for invalid credentials");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your username is invalid"),
                    "Error message should indicate invalid username");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Invalid Username");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with invalid password")]
        public void Test_LoginWithInvalidPassword_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Invalid Password");
            string username = ConfigReader.GetValidUsername();
            string password = ConfigReader.GetInvalidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for invalid credentials");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your password is invalid"),
                    "Error message should indicate invalid password");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Invalid Password");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with both invalid username and password")]
        public void Test_LoginWithInvalidCredentials_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Invalid Username and Password");
            string username = ConfigReader.GetInvalidUsername();
            string password = ConfigReader.GetInvalidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for invalid credentials");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("invalid"),
                    "Error message should indicate invalid credentials");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Invalid Username and Password");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with empty username")]
        public void Test_LoginWithEmptyUsername_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Empty Username");
            string username = string.Empty;
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed when username is empty");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your username is invalid"),
                    "Error message should indicate invalid username");

                Assert.That(_loginPage.GetCurrentUrl(), Does.Not.Contain("logged-in-successfully"),
                    "URL should not navigate to success page");
            });

            Logger.Info("Test Passed: Login with Empty Username");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with empty password")]
        public void Test_LoginWithEmptyPassword_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Empty Password");
            string username = ConfigReader.GetValidUsername();
            string password = string.Empty;

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed when password is empty");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your password is invalid"),
                    "Error message should indicate invalid password");

                Assert.That(_loginPage.GetCurrentUrl(), Does.Not.Contain("logged-in-successfully"),
                    "URL should not navigate to success page");
            });

            Logger.Info("Test Passed: Login with Empty Password");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that user cannot login with empty credentials")]
        public void Test_LoginWithEmptyCredentials_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Empty Credentials");
            string username = string.Empty;
            string password = string.Empty;

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed when credentials are empty");

                Assert.That(_loginPage.GetCurrentUrl(), Does.Not.Contain("logged-in-successfully"),
                    "URL should not navigate to success page");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Empty Credentials");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that username field is case-sensitive")]
        public void Test_LoginWithCaseSensitiveUsername_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Case Sensitive Username");
            string username = "STUDENT"; // Using uppercase
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for case-sensitive username mismatch");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your username is invalid"),
                    "Error message should indicate invalid username");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Case Sensitive Username");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that password field is case-sensitive")]
        public void Test_LoginWithCaseSensitivePassword_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Case Sensitive Password");
            string username = ConfigReader.GetValidUsername();
            string password = "password123"; // Using lowercase

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for case-sensitive password mismatch");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your password is invalid"),
                    "Error message should indicate invalid password");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Case Sensitive Password");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that username field does not accept leading/trailing spaces")]
        public void Test_LoginWithUsernameSpaces_Failure()
        {
            // Arrange
            Logger.Info("Starting Test: Login with Username Spaces");
            string username = " student "; // Username with spaces
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                    "Error message should be displayed for username with spaces");

                Assert.That(_loginPage.GetErrorMessage(), Does.Contain("Your username is invalid"),
                    "Error message should indicate invalid username");

                Assert.That(_loginPage.IsLogoutButtonDisplayed(), Is.False,
                    "Logout button should not be displayed after failed login");
            });

            Logger.Info("Test Passed: Login with Username Spaces");
        }

        [Test]
        [Category("Regression")]
        [Category("Login")]
        [Description("Verify that success message contains congratulations text")]
        public void Test_SuccessMessageContent_Verification()
        {
            // Arrange
            Logger.Info("Starting Test: Success Message Content Verification");
            string username = ConfigReader.GetValidUsername();
            string password = ConfigReader.GetValidPassword();

            // Act
            _loginPage.NavigateToLoginPage();
            _loginPage.Login(username, password);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_loginPage.GetSuccessMessage(), Does.Contain("Congratulations").Or.Contains("Successfully"),
                    "Success message should contain 'Congratulations' or 'successfully logged in'");

                Assert.That(_loginPage.IsSuccessMessageDisplayed(), Is.True,
                    "Success message should be visible on the page");
            });

            Logger.Info("Test Passed: Success Message Content Verification");
        }
    }
}