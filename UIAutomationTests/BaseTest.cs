using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace UIAutomationTests
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;
        protected string BaseUrl = "http://localhost:5232";

        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();

            // ⛔ Remove o modo headless se estiveres a depurar problemas visuais
            // Descomenta apenas se queres ver a execução real:
            // options.AddArgument("--headless"); 
            
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            Driver = new ChromeDriver(options);

            // 🔄 Ajuste recomendado: diminuir implicit wait para evitar conflitos com explicit waits
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}