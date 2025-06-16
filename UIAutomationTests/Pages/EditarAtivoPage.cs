using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class EditarAtivoPage
    {
        private readonly IWebDriver _driver;

        public EditarAtivoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement DataInicio => _driver.FindElement(By.Name("AtivoFinanceiro.DataInicio"));
        public IWebElement Duracao => _driver.FindElement(By.Name("AtivoFinanceiro.Duracao"));
        public IWebElement Imposto => _driver.FindElement(By.Name("AtivoFinanceiro.Imposto"));
        public IWebElement Valor => _driver.FindElement(By.Name("DepositoPrazo.Valor"));
        public IWebElement Banco => _driver.FindElement(By.Name("DepositoPrazo.Banco"));
        public IWebElement NumeroConta => _driver.FindElement(By.Name("DepositoPrazo.NumeroConta"));
        public IWebElement Titulares => _driver.FindElement(By.Name("DepositoPrazo.Titulares"));
        public IWebElement TaxaJuro => _driver.FindElement(By.Name("DepositoPrazo.TaxaJuroAnual"));
        public IWebElement Submit => _driver.FindElement(By.CssSelector("button[type='submit']"));
        public IWebElement Cancelar => _driver.FindElement(By.CssSelector("a.btn.btn-secondary"));
        public IWebElement ErrorMessage => _driver.FindElement(By.CssSelector(".alert.alert-danger"));
    }
}