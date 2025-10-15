using NUnit.Framework;
using UIAuto.Pages;
using UIAuto.Utilities;

namespace UIAuto.Tests
{
    [TestFixture]
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
    }
}