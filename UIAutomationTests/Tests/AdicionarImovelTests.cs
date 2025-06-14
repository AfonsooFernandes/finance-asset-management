using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class AdicionarImovelTests : BaseTest
    {
        [Test]
        public void TestAdicionarImovelComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("teste@ipvc.pt", "testepassword");

            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '27');");
            Thread.Sleep(1000);

            Driver.Navigate().GoToUrl($"{BaseUrl}/AdicionarImovel");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Id("Ativo_DataInicio")).Displayed);

            var page = new AdicionarImovelPage(Driver);

            page.DataInicio.SendKeys("2025-06-14");
            page.Duracao.SendKeys("36");
            page.Imposto.SendKeys("12");
            page.Designacao.SendKeys("Apartamento Centro");
            page.Localizacao.SendKeys("Rua Exemplo, Viana");
            page.ValorImovel.SendKeys("150000");
            page.ValorRenda.SendKeys("750");
            page.ValorCondominio.SendKeys("50");
            page.OutrasDespesas.SendKeys("100");

            page.Submit.Click();
        }
    }
}