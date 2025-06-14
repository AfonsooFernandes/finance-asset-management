using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class ListarAtivosPage
    {
        private readonly IWebDriver _driver;

        public ListarAtivosPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement TabelaAtivos => _driver.FindElement(By.TagName("table"));
        public IWebElement VoltarButton => _driver.FindElement(By.LinkText("Voltar ao Menu"));
    }
}