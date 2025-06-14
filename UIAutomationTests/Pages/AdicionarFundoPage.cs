using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class AdicionarFundoPage
    {
        private readonly IWebDriver _driver;

        public AdicionarFundoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement DataInicio => _driver.FindElement(By.Id("Ativo_DataInicio"));
        public IWebElement Duracao => _driver.FindElement(By.Id("Ativo_Duracao"));
        public IWebElement Imposto => _driver.FindElement(By.Id("Ativo_Imposto"));

        public IWebElement Nome => _driver.FindElement(By.Id("Fundo_Nome"));
        public IWebElement Montante => _driver.FindElement(By.Id("Fundo_Montante"));
        public IWebElement TaxaJuro => _driver.FindElement(By.Id("Fundo_TaxaJuro"));

        public IWebElement Submit => _driver.FindElement(By.CssSelector("button[type='submit']"));
    }
}