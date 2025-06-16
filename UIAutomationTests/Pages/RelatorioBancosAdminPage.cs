using OpenQA.Selenium;
using System.Collections.Generic;

namespace UIAutomationTests.Pages
{
    public class RelatorioBancosAdminPage
    {
        private readonly IWebDriver _driver;

        public RelatorioBancosAdminPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement Title => _driver.FindElement(By.TagName("h2"));
        public IReadOnlyCollection<IWebElement> TableRows => _driver.FindElements(By.CssSelector("table tbody tr"));
        public IWebElement BackButton => _driver.FindElement(By.CssSelector("a.btn.btn-secondary"));
        
        public string GetBancoFromRow(int rowIndex)
        {
            return TableRows.ElementAt(rowIndex).FindElements(By.TagName("td"))[0].Text;
        }

        public string GetTotalDepositadoFromRow(int rowIndex)
        {
            return TableRows.ElementAt(rowIndex).FindElements(By.TagName("td"))[1].Text;
        }

        public string GetJurosTotaisPagosFromRow(int rowIndex)
        {
            return TableRows.ElementAt(rowIndex).FindElements(By.TagName("td"))[2].Text;
        }
    }
}