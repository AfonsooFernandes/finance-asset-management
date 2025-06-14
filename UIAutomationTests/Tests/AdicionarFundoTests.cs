using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class AdicionarFundoTests : BaseTest
    {
        [Test]
        public void TestAdicionarFundoComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("teste@ipvc.pt", "testepassword");

            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '27');");
            Thread.Sleep(1000);

            Driver.Navigate().GoToUrl($"{BaseUrl}/AdicionarFundo");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Id("Ativo_DataInicio")).Displayed);

            var page = new AdicionarFundoPage(Driver);

            page.DataInicio.SendKeys("2025-06-14");
            page.Duracao.SendKeys("18");
            page.Imposto.SendKeys("8");
            page.Nome.SendKeys("Fundo XPTO");
            page.Montante.SendKeys("5000");
            page.TaxaJuro.SendKeys("5");

            page.Submit.Click();
        }
    }
}