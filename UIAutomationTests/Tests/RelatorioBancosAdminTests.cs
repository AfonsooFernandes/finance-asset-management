using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class RelatorioBancosAdminTests : BaseTest
    {
        [Test]
        public void TestCarregarRelatorioBancosAdminComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("admin@ipvc.pt", "Password123");
            
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '1');");
            Thread.Sleep(1000);
            
            Driver.Navigate().GoToUrl($"{BaseUrl}/RelatorioBancosAdmin/1");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.TagName("h2")).Displayed);

            var page = new RelatorioBancosAdminPage(Driver);
            
            Assert.AreEqual("Relatório de Bancos", page.Title.Text, "O título da página não corresponde ao esperado.");
            
            Assert.IsTrue(page.TableRows.Count > 0, "A tabela não contém dados.");
            
            Assert.IsNotEmpty(page.GetBancoFromRow(0), "O campo Banco está vazio.");
            Assert.IsNotEmpty(page.GetTotalDepositadoFromRow(0), "O campo Total Depositado está vazio.");
            Assert.IsNotEmpty(page.GetJurosTotaisPagosFromRow(0), "O campo Juros Totais Pagos está vazio.");
            
            wait.Until(driver => page.BackButton.Displayed);
            page.BackButton.Click();
            wait.Until(driver => driver.Url.Contains("/Menu"));
            Assert.IsTrue(Driver.Url.Contains("/Menu"), "O botão Voltar não redirecionou para a página Menu.");
        }
    }
}