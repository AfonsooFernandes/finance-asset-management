using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class AdicionarDepositoPage
    {
        private readonly IWebDriver _driver;

        public AdicionarDepositoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement DataInicio => _driver.FindElement(By.Id("Ativo_DataInicio"));
        public IWebElement Duracao => _driver.FindElement(By.Id("Ativo_Duracao"));
        public IWebElement Imposto => _driver.FindElement(By.Id("Ativo_Imposto"));
        public IWebElement Valor => _driver.FindElement(By.Id("Deposito_Valor"));
        public IWebElement Banco => _driver.FindElement(By.Id("Deposito_Banco"));
        public IWebElement NumeroConta => _driver.FindElement(By.Id("Deposito_NumeroConta"));
        public IWebElement Titulares => _driver.FindElement(By.Id("Deposito_Titulares"));
        public IWebElement TaxaJuro => _driver.FindElement(By.Id("Deposito_TaxaJuroAnual"));
        public IWebElement Submit => _driver.FindElement(By.CssSelector("button[type='submit']"));
    }
}