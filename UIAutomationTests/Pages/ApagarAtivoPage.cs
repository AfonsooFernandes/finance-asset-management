using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class ApagarAtivoPage
    {
        private readonly IWebDriver _driver;

        public ApagarAtivoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement ConfirmarApagarButton => _driver.FindElement(By.CssSelector("button.btn.btn-primary"));
        public IWebElement CancelarButton => _driver.FindElement(By.CssSelector("a.btn.btn-secondary"));
        public IWebElement WarningMessage => _driver.FindElement(By.CssSelector("div.alert.alert-warning"));
        public IWebElement ErrorMessage => _driver.FindElement(By.CssSelector("div.alert.alert-danger"));
    }
}