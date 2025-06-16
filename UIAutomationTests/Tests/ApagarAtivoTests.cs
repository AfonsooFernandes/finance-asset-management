using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class ApagarAtivoTests : BaseTest
    {
        [Test]
        public void TestApagarAtivoComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "Password123");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '11');");
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return localStorage.getItem('userId') === '11';"));
            
            Driver.Navigate().GoToUrl($"{BaseUrl}/ApagarAtivo?id=22&userId=11");
            
            wait.Until(driver => 
            {
                try
                {
                    return driver.FindElement(By.CssSelector("div.alert.alert-warning")).Displayed;
                }
                catch (WebDriverException)
                {
                    return false;
                }
            });

            var page = new ApagarAtivoPage(Driver);
            
            wait.Until(driver => 
            {
                try
                {
                    var confirmButton = page.ConfirmarApagarButton;
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmButton);
                    return confirmButton.Displayed && confirmButton.Enabled;
                }
                catch (WebDriverException)
                {
                    return false;
                }
            });
            
            try
            {
                page.ConfirmarApagarButton.Click();
            }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", page.ConfirmarApagarButton);
            }
            
            wait.Until(driver => driver.Url.Contains("/Listar_Ativos"));
        }
    }
}