using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class RelatorioAtivosPage
    {
        private readonly IWebDriver _driver;

        public RelatorioAtivosPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement ErrorMessage => _driver.FindElement(By.CssSelector(".alert.alert-warning"));
        public IWebElement Table => _driver.FindElement(By.CssSelector("table.table.table-striped.table-bordered"));
        public IWebElement BackButton => _driver.FindElement(By.CssSelector("a.btn.btn-secondary"));

        public IWebElement TableRows =>_driver.FindElement(By.CssSelector("table.table.table-striped.table-bordered tbody tr"));
    }
}