using NUnit.Framework;
using UIAutomationTests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace UIAutomationTests.Tests
{
    public class ListarAtivosTests : BaseTest
    {
        [Test]
        public void TestTabelaDeAtivosEhVisivel()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("teste@ipvc.pt", "Password123");

            // ⚠️ Define o userId no localStorage (necessário se a página depende disso)
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '27');");

            Driver.Navigate().GoToUrl($"{BaseUrl}/Listar_Ativos?userId=27");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.CssSelector("table")).Displayed);

            var page = new ListarAtivosPage(Driver);
            Assert.That(page.TabelaAtivos.Displayed, Is.True);
        }
    }
}