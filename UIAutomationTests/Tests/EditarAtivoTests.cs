using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class EditarAtivoTests : BaseTest
    {
        [Test]
        public void TestEditarDepositoPrazoComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "Password123");
            
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '11');");
            Thread.Sleep(1000);
            
            Driver.Navigate().GoToUrl($"{BaseUrl}/EditarAtivo?tipo=depositoprazo&id=18&userId=11");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Name("AtivoFinanceiro.DataInicio")).Displayed);

            var page = new EditarAtivoPage(Driver);

            // Clear and fill form fields
            page.DataInicio.Clear();
            page.DataInicio.SendKeys("2025-06-15");
            page.Duracao.Clear();
            page.Duracao.SendKeys("25");
            page.Imposto.Clear();
            page.Imposto.SendKeys("5.5");
            page.Valor.Clear();
            page.Valor.SendKeys("300");
            page.Banco.Clear();
            page.Banco.SendKeys("Caixa Geral");
            page.NumeroConta.Clear();
            page.NumeroConta.SendKeys("987654321");
            page.Titulares.Clear();
            page.Titulares.SendKeys("Luis Pedro");
            page.TaxaJuro.Clear();
            page.TaxaJuro.SendKeys("5.75");
            
            wait.Until(driver => page.Submit.Displayed && page.Submit.Enabled);
            page.Submit.Click();
            
            wait.Until(driver => driver.Url.Contains("/DetalhesAtivo"));
        }
    }
}