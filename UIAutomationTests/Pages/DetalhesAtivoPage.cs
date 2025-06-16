using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class DetalhesAtivoPage
    {
        private readonly IWebDriver _driver;

        public DetalhesAtivoPage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        public IWebElement Tipo => _driver.FindElement(By.XPath("//dt[text()='Tipo']/following-sibling::dd"));
        public IWebElement DataInicio => _driver.FindElement(By.XPath("//dt[text()='Data Início']/following-sibling::dd"));
        public IWebElement Duracao => _driver.FindElement(By.XPath("//dt[text()='Duração']/following-sibling::dd"));
        public IWebElement Imposto => _driver.FindElement(By.XPath("//dt[text()='Imposto']/following-sibling::dd"));
        public IWebElement ImpostoAnual => _driver.FindElement(By.XPath("//dt[text()='Imposto Anual']/following-sibling::dd"));
        public IWebElement JurosAnuais => _driver.FindElement(By.XPath("//dt[text()='Juros Anuais']/following-sibling::dd"));
        
        public IWebElement Valor => _driver.FindElement(By.XPath("//dt[text()='Valor']/following-sibling::dd"));
        public IWebElement Banco => _driver.FindElement(By.XPath("//dt[text()='Banco']/following-sibling::dd"));
        public IWebElement NumeroConta => _driver.FindElement(By.XPath("//dt[text()='Número da Conta']/following-sibling::dd"));
        public IWebElement Titulares => _driver.FindElement(By.XPath("//dt[text()='Titulares']/following-sibling::dd"));
        public IWebElement TaxaJuroAnual => _driver.FindElement(By.XPath("//dt[text()='Taxa de Juro Anual']/following-sibling::dd"));
        
        public IWebElement VoltarButton => _driver.FindElement(By.CssSelector("a.btn.btn-secondary[href='/Menu']"));
        public IWebElement EditarButton => _driver.FindElement(By.CssSelector("form[action='/EditarAtivo'] button.btn.btn-primary"));
        public IWebElement EliminarButton => _driver.FindElement(By.CssSelector("form[action='/ApagarAtivo'] button.btn.btn-primary"));
        
        public IWebElement ErrorMessage => _driver.FindElement(By.CssSelector("div.alert.alert-danger"));
    }
}