using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class AdicionarDepositoTests : BaseTest
    {
        [Test]
        public void TestAdicionarDepositoPrazoComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("teste@ipvc.pt", "testepassword");

            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '27');");
            Thread.Sleep(1000); 

            Driver.Navigate().GoToUrl($"{BaseUrl}/AdicionarDeposito");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Id("Ativo_DataInicio")).Displayed);

            var page = new AdicionarDepositoPage(Driver);

            page.DataInicio.SendKeys("2025-06-14");
            page.Duracao.SendKeys("12");
            page.Imposto.SendKeys("6");
            page.Valor.SendKeys("100");
            page.Banco.SendKeys("Santander");
            page.NumeroConta.SendKeys("2132131");
            page.Titulares.SendKeys("Afonso");
            page.TaxaJuro.SendKeys("6");

            wait.Until(driver => page.Submit.Displayed); 
            page.Submit.Click();
        }
    }
}