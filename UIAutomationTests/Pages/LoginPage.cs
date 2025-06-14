using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver Driver;

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebElement Email => Driver.FindElement(By.Id("email"));
        public IWebElement Password => Driver.FindElement(By.Id("palavra-passe"));
        public IWebElement Submit => Driver.FindElement(By.CssSelector("form button[type='submit']"));
        public IWebElement ErrorMessage => Driver.FindElement(By.Id("errorMessage"));

        public void Login(string email, string password)
        {
            Email.Clear();
            Email.SendKeys(email);
            Password.Clear();
            Password.SendKeys(password);
            Submit.Click();
        }
    }
}