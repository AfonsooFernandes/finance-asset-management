using NUnit.Framework;
using UIAutomationTests.Pages;
using OpenQA.Selenium;

namespace UIAutomationTests.Tests
{
    public class NavegacaoMenuTests : BaseTest
    {
        [Test]
        public void TestNavegarParaListarAtivos()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var login = new LoginPage(Driver);
            login.Login("teste@ipvc.pt", "testepassword");
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '27');");

            var menu = new MenuPage(Driver);
            menu.ListarAtivosButton.Click();

            Assert.That(Driver.Url, Does.Contain("/Listar_Ativos"));
        }
    }
}