using NUnit.Framework; 
using OpenQA.Selenium; 
using OpenQA.Selenium.Support.UI; 
using System; 
using UIAutomationTests.Pages;

namespace UIAutomationTests.Tests
{
    public class RelatorioAtivosTests : BaseTest
    {
        [Test]
        public void TestVisualizarRelatorioAtivosComSucesso()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Login");
            var loginPage = new LoginPage(Driver);
            loginPage.Login("paivaluis@ipvc.pt", "Password123");
            
            ((IJavaScriptExecutor)Driver).ExecuteScript("localStorage.setItem('userId', '11');");
            System.Threading.Thread.Sleep(1000);
            
            Driver.Navigate().GoToUrl($"{BaseUrl}/RelatorioAtivos?userId=11");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.CssSelector("table.table.table-striped.table-bordered")).Displayed || driver.FindElement(By.CssSelector(".alert.alert-warning")).Displayed);

            var page = new RelatorioAtivosPage(Driver);
            
            if (page.Table.Displayed)
            {
                Assert.IsTrue(page.TableRows.Displayed, "Table rows should be displayed when assets are present.");
            }
            else
            {
                Assert.IsTrue(page.ErrorMessage.Displayed, "Error message should be displayed when no assets are found.");
                Assert.That(page.ErrorMessage.Text, Is.EqualTo("Nenhum ativo encontrado para este utilizador."), "Error message text should match the expected value.");
            }
            
            wait.Until(driver => page.BackButton.Displayed);
            page.BackButton.Click();
            wait.Until(driver => driver.Url.Contains("/Menu"));
            Assert.IsTrue(Driver.Url.Contains("/Menu"), "Should navigate to Menu page after clicking Voltar.");
        }
    }
}