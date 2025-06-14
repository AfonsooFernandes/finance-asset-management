using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class RegisterPage
    {
        private readonly IWebDriver _driver;

        public RegisterPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement NomeInput => _driver.FindElement(By.Id("nome"));
        public IWebElement EmailInput => _driver.FindElement(By.Id("email"));
        public IWebElement SenhaInput => _driver.FindElement(By.Id("senha"));
        public IWebElement SubmitButton => _driver.FindElement(By.CssSelector("button[type='submit']"));
        public IWebElement ErrorMessage => _driver.FindElement(By.Id("errorMessage"));

        public void Register(string nome, string email, string senha)
        {
            NomeInput.SendKeys(nome);
            EmailInput.SendKeys(email);
            SenhaInput.SendKeys(senha);
            SubmitButton.Click();
        }
    }
}