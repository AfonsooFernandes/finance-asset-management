using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class MenuPage
    {
        private readonly IWebDriver _driver;

        public MenuPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement ListarAtivosButton => _driver.FindElement(By.Id("listarAtivosLink"));
        public IWebElement RelatorioAtivosButton => _driver.FindElement(By.Id("relatorioAtivosLink"));
        public IWebElement RelatorioImpostosButton => _driver.FindElement(By.Id("relatorioImpostosLink"));
    }
}