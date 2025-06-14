using OpenQA.Selenium;

namespace UIAutomationTests.Pages
{
    public class AdicionarImovelPage
    {
        private readonly IWebDriver _driver;

        public AdicionarImovelPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement DataInicio => _driver.FindElement(By.Id("Ativo_DataInicio"));
        public IWebElement Duracao => _driver.FindElement(By.Id("Ativo_Duracao"));
        public IWebElement Imposto => _driver.FindElement(By.Id("Ativo_Imposto"));

        public IWebElement Designacao => _driver.FindElement(By.Id("Imovel_Designacao"));
        public IWebElement Localizacao => _driver.FindElement(By.Id("Imovel_Localizacao"));
        public IWebElement ValorImovel => _driver.FindElement(By.Id("Imovel_ValorImovel"));
        public IWebElement ValorRenda => _driver.FindElement(By.Id("Imovel_ValorRenda"));
        public IWebElement ValorCondominio => _driver.FindElement(By.Id("Imovel_ValorCondominio"));
        public IWebElement OutrasDespesas => _driver.FindElement(By.Id("Imovel_OutrasDespesas"));

        public IWebElement Submit => _driver.FindElement(By.CssSelector("button[type='submit']"));
    }
}