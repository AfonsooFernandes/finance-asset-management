using NUnit.Framework;
using UIAutomationTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace UIAutomationTests.Tests
{
    public class LoginTests : BaseTest
    {
        [Test]
        public void TestSuccessfulLoginRedirectsToMenu()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "Password123");

            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(driver =>driver.Url.Contains("/Menu"));

            Assert.That(Driver.Url, Does.Contain("/Menu"));
        }

        [Test]
        public void TestLoginWithInvalidPasswordShowsError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "wrongpass");

            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(driver =>
                loginPage.ErrorMessage.Displayed && loginPage.ErrorMessage.Text.Contains("Erro")
            );

            Assert.That(loginPage.ErrorMessage.Text, Does.Contain("Erro"));
        }
    }
}