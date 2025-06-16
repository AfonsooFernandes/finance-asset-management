using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class DetalhesAtivoTests : BaseTest
    {
        [Test]
        public void TestExibirDetalhesDepositoPrazoComSucesso()
        {
            // Arrange: Login
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "Password123");

            // Set userId in localStorage
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '11');");
            System.Threading.Thread.Sleep(1000);
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            
            int ativoId = 17;
            Driver.Navigate().GoToUrl($"{BaseUrl}/DetalhesAtivo?tipo=depositoprazo&id={ativoId}&userId=11");

            wait.Until(driver => driver.FindElement(By.XPath("//dt[text()='Tipo']/following-sibling::dd")).Displayed);
            var detalhesPage = new DetalhesAtivoPage(Driver);
            
            Assert.That(detalhesPage.Tipo.Text, Is.EqualTo("Depósito a Prazo"), "Tipo should be Depósito a Prazo");
            Assert.That(detalhesPage.DataInicio.Text, Is.EqualTo("11/06/2025"), "Data Início should match");
            Assert.That(detalhesPage.Duracao.Text, Is.EqualTo("16"), "Duração should match");
            Assert.That(detalhesPage.Imposto.Text, Is.EqualTo("21"), "Imposto should match");
            Assert.That(detalhesPage.JurosAnuais.Text, Is.EqualTo("148148,04 €"), "Juros Anuais should match");
            Assert.That(detalhesPage.Valor.Text, Is.EqualTo("1234567,00"), "Valor should match");
            Assert.That(detalhesPage.Banco.Text, Is.EqualTo("Millenium"), "Banco should match");
            Assert.That(detalhesPage.NumeroConta.Text, Is.EqualTo("123456789"), "Número da Conta should match");
            Assert.That(detalhesPage.Titulares.Text, Is.EqualTo("Miguel"), "Titulares should match");
            Assert.That(detalhesPage.TaxaJuroAnual.Text, Is.EqualTo("12,00%"), "Taxa de Juro Anual should match");
        }
    }
}