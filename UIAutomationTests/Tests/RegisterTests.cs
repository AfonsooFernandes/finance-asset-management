using NUnit.Framework;
using UIAutomationTests.Pages;
using System;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace UIAutomationTests.Tests
{
    public class RegisterTests : BaseTest
    {
        [Test]
        public void TestUserRegistrationRedirectsToLogin()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Register");
            var registerPage = new RegisterPage(Driver);
            string timestamp = DateTime.Now.Ticks.ToString();
            registerPage.Register("Teste User", $"teste_{timestamp}@ipvc.pt", "Password123");

            new WebDriverWait(Driver, TimeSpan.FromSeconds(5))
                .Until(d => d.Url.ToLower().Contains("/login"));

            Assert.That(Driver.Url.ToLower(), Does.Contain("/login"));
        }
    }
}