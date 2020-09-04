using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace NerdStore.BDD.Tests.Config
{
    public class SeleniumHelper : IDisposable
    {
        public IWebDriver WebDriver;
        public readonly ConfigurationHelper Configuration;
        public WebDriverWait Wait;

        public SeleniumHelper(Browser browser, ConfigurationHelper configuration, bool headless = true)
        {
            Configuration = configuration;
            WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDrivers, headless);

            WebDriver.Manage().Window.Maximize();
            Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
        }

        public string ObterUrl() => WebDriver.Url;

        public void IrParaUrl(string url) 
            => WebDriver.Navigate().GoToUrl(url);

        public bool ValidarConteudoUrl(string conteudo)
            => Wait.Until(ExpectedConditions.UrlContains(conteudo));

        public void ClicarLinkPorTexto(string linkText) 
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText))).Click();

        public void ClicarBotaoPorId(string botaoId)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(botaoId))).Click();

        public void ClicarPorXPath(string xPath)
           => Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath))).Click();

        public IWebElement ObterElementoPorClase(string classeCss)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(classeCss)));

        public IWebElement ObterElementoPorXPath(string xPath)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));

        public void PreencherTextBoxPorId(string idCampo, string valorCampo)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo))).SendKeys(valorCampo);

        public void PreencherDropDownPorId(string idCampo, string valorCampo)
        {
            var campo =  Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            var selectElement = new SelectElement(campo);
            selectElement.SelectByValue(valorCampo);
        }

        public string ObterTextoElementoPorClasseCss(string className)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className))).Text;

        public string ObterTextoElementoPorId(string id)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;

        public string ObterValorTextBoxPorId(string id)
            => Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).GetAttribute("value");

        public IEnumerable<IWebElement> ObterListaPorClasse(string className)
            => Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));

        public bool ValidarSeOElementoExistePorXPath(string xPath)
            => ElementoExiste(By.XPath(xPath));

        public bool ValidarSeOElementoExistePorId(string id)
            => ElementoExiste(By.Id(id));

        public void VoltarNavegacao(int vezes = 1)
        {
            for(var i = 0; i < vezes; i++)
            {
                WebDriver.Navigate().Back();
            }
        }

        public void ObterScreenShot(string nome)
            => SalvarScreenShot(WebDriver.TakeScreenshot(), $"{DateTime.Now.ToFileTime()}_{nome}.png");

        private void SalvarScreenShot(Screenshot screenshot, string fileName)
            => screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}", ScreenshotImageFormat.Png);

        private bool ElementoExiste(By by)
        {
            try
            {
                WebDriver.FindElement(by);
                return true;
            }
            catch(NoSuchElementException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
